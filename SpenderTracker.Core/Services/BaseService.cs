using Microsoft.EntityFrameworkCore;
using SpenderTracker.Core.Interfaces;
using SpenderTracker.Data.Context;
using SpenderTracker.Data.Interface;

namespace SpenderTracker.Core.Services;

public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
    where TEntity : class, IEntity<TDto>, new()
    where TDto : class, IDto
{
    protected readonly ApplicationContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseService(ApplicationContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>(); 
    }

    public async virtual Task<TDto?> GetById(int id, CancellationToken ct)
    {
        try
        {
            TEntity? entity = await _dbSet.FindAsync([id], ct);
            if (entity == null) return null;
            return entity.ToDto(); 
        } catch(OperationCanceledException)
        {
            return null;
        } 
    }

    public async virtual Task<List<TDto>?> GetAll(CancellationToken ct)
    {
        try
        {
            var dtos = await _dbSet.AsNoTracking().
                Select(e => e.ToDto()).
                ToListAsync(ct);
            return dtos; 
        } catch(OperationCanceledException)
        {
            return null;
        }
    }

    public async virtual Task<TDto?> Insert(TDto dto, CancellationToken ct)
    {
        TEntity entity = new();
        _dbSet.Entry(entity).CurrentValues.SetValues(dto); 
        _dbSet.Add(entity);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
            return entity.ToDto();
        } catch(DbUpdateException)
        {
            return null; 
        } catch (OperationCanceledException)
        {
            return null;
        }
    }

    public virtual async Task<bool> Update(TDto dto, CancellationToken ct)
    {
        TEntity? entity = await _dbSet.FindAsync([dto.Id], ct);
        if (entity == null) return false;

        _dbSet.Entry(entity).CurrentValues.SetValues(dto);
        if (!_dbContext.ChangeTracker.HasChanges()) return true; 

        try
        {
            await _dbContext.SaveChangesAsync(ct);
            return true; 
        } catch (DbUpdateException)
        {
            return false; 
        } catch (OperationCanceledException)
        {
            return false;
        }
    }

    public virtual async Task<bool> Delete(int id, CancellationToken ct)
    { 
        try
        {
            await _dbSet.Where(e => e.Id == id).ExecuteDeleteAsync(ct);
            return true;
        } catch (DbUpdateException)
        {
            return false; 
        } catch (OperationCanceledException)
        {
            return false;
        }
    } 

    public virtual async Task<bool> DoesExist(int id, CancellationToken ct)
    {
        try
        {
            bool doesExist = await _dbSet.AsNoTracking().
                AnyAsync(e => e.Id == id, ct);
            return doesExist;
        } catch(OperationCanceledException)
        {
            return false;
        } 
    }
}
