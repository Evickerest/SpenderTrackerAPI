using SpenderTracker.Data.Interface;

namespace SpenderTracker.Core.Interfaces;

public interface IBaseService<TEntity, TDto>
    where TEntity : IEntity<TDto>, new()
    where TDto : IDto
{
    Task<TDto?> GetById(int id, CancellationToken ct);
    Task<List<TDto>?> GetAll(CancellationToken ct);
    Task<TDto?> Insert(TDto dto, CancellationToken ct);
    Task<bool> Update(TDto dto, CancellationToken ct);
    Task<bool> Delete(int id, CancellationToken ct);
    Task<bool> DoesExist(int id, CancellationToken ct);
}
