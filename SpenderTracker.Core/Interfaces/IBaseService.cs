using SpenderTracker.Data.Interface;

namespace SpenderTracker.Core.Interfaces;

public interface IBaseService<TEntity, TDto>
    where TEntity : IEntity<TDto>, new()
    where TDto : IDto
{
    TDto? GetById(int id);
    List<TDto> GetAll();
    TDto? Insert(TDto dto);
    bool Update(TDto dto);
    bool Delete(TDto dto);
}
