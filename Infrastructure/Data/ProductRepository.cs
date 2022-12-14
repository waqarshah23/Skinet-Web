using Core.Interfaces;
using Core.Models;
using Infrastructure.Middleware;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository: IProductRepository
    {
        protected readonly DataContext _context;
        public ProductRepository(DataContext context )
        {
            _context = context;
        }

        public async Task<Products> getProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Products>> getProductsListAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
