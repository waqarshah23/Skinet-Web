namespace PTO_Server.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetListAsync();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);

    }
}
