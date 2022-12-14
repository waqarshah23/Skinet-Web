using Core.Models;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Products> getProductByIdAsync(int id);
        Task<IReadOnlyList<Products>> getProductsListAsync();
    }
}
