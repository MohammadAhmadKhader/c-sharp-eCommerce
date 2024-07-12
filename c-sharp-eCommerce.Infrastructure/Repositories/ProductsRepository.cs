using AutoMapper;
using c_shap_eCommerce.Core.DTOs;
using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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
        public ProductsRepository(AppDbContext AppDbContext) : base(AppDbContext)
        {
            this.appDbContext = AppDbContext;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId) {

            // Eager Loading
            var products = await appDbContext.Products
                .Where(prod => prod.CategoryId == categoryId)
                .Include(prod => prod.Category)
                .ToListAsync();

            return products;
        }
        
    }
}
