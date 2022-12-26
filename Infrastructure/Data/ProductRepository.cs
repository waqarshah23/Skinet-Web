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
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Products>> getProductsListAsync()
        {
            return await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductBrand>> getProductsBrandsAsync()
        {
            return await _context.productBrand.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> getProductTypesAsync()
        {
            return await _context.productType.ToListAsync();
        }
    }
}
