using Microsoft.EntityFrameworkCore;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Context;
using SpenderTracker.Data.Interface;

namespace SpenderTracker.Core.Services;

public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
    where TEntity : class, IEntity<TDto>, new()
    where TDto : class, IDto
{
    private readonly ApplicationContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public BaseService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>(); 
    }

    public virtual TDto? GetById(int id)
    {
        TEntity? entity = _dbSet.Find(id);
        if (entity == null) return null;
        return entity.ToDto(); 
    }

    public virtual List<TDto> GetAll()
    {
        return _dbSet.AsNoTracking().
            Select(e => e.ToDto()).
            ToList();
    }

    public virtual TDto? Insert(TDto dto)
    {
        TEntity entity = new();
        _dbSet.Entry(entity).CurrentValues.SetValues(dto);
        _dbSet.Add(entity);
        
        if (_dbContext.SaveChanges() > 0)
        {
            return entity.ToDto();
        } else
        {
            return null;
        }
    }

    public virtual bool Update(TDto dto)
    {
        TEntity? entity = _dbSet.Find(dto.Id);
        if (entity == null) return false;

        _dbSet.Entry(entity).CurrentValues.SetValues(dto);
        return _dbContext.SaveChanges() > 0;
    }

    public virtual bool Delete(TDto dto)
    {
        TEntity? entity = _dbSet.Find(dto.Id);
        if (entity == null) return false;

        _dbSet.Remove(entity);
        return _dbContext.SaveChanges() > 0;

    } 
}
