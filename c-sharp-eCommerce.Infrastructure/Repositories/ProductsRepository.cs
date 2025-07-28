using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class ProductsRepository : GenericRepository<Product>, IProductsRepository
    {
        private readonly AppDbContext _context;
        public ProductsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId, int page, int limit)
        {

            var skip = (page - 1) * limit;

            var products = await _context.Products
                .Where(prod => prod.CategoryId == categoryId)
                .Include(prod => prod.Category)
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            return products;
        }
        public async Task<Product?> GetProductById(int Id)
        {
            var product = await _context.Products
                .Include(prod => prod.Category)
                .FirstOrDefaultAsync(x => x.Id == Id);
            return product;
        }

        public async Task<List<Product>> GetProductsByIds(List<int> ListOfIds)
        {
            var products = await _context.Products
                .Where(prod => ListOfIds.Contains(prod.Id))
                .ToListAsync();
            return products;
        }

    }
}
