using AutoMapper;
using c_shap_eCommerce.Core.DTOs;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class ProductsRepository : GenericRepository<Product>, IProductsRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IServiceScopeFactory serviceScopeFactory;
		public ProductsRepository(AppDbContext AppDbContext, IServiceScopeFactory serviceScopeFactory) : base(AppDbContext, serviceScopeFactory)
        {
            this.appDbContext = AppDbContext;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId, int page, int limit) {

            var skip = (page - 1) * limit;
            // Eager Loading
            var products = await appDbContext.Products
                .Where(prod => prod.CategoryId == categoryId)
                .Include(prod => prod.Category)
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            return products;
        }
        public async Task<Product> GetProductById(int Id)
        {
            var product = await appDbContext.Products
                .Include(prod => prod.Category)
                .FirstOrDefaultAsync(x=>x.Id == Id);
            return product;
        }

        public async Task<List<Product>> GetProductsByIds(List<int> ListOfIds)
        {
            var products = await appDbContext.Products
                .Where(prod => ListOfIds.Contains(prod.Id))
                .ToListAsync();
            return products;
        }
        
    }
}
