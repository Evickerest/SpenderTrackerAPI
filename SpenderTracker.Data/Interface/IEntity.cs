 namespace SpenderTracker.Data.Interface;

public interface IEntity<TDto>
    where TDto : IDto
{
    int Id { get; set; }
    TDto ToDto(); 
}
