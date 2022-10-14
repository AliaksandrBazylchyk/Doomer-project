namespace Application.Repository
{
    public interface IRepository<TEntity> 
        where TEntity : class
    {
        TEntity Get();
        IEnumerable<TEntity> GetAll();
    }
}
