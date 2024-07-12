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
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly AppDbContext appDbContext;
        public GenericRepository(AppDbContext AppDbContext) {
            this.appDbContext = AppDbContext;
        }
        

        public async Task Create(TModel model)
        {
            await appDbContext.Set<TModel>().AddAsync(model);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await appDbContext.Set<TModel>().FindAsync(id);
            if(model is null)
            {
                return false;
            }
            appDbContext.Set<TModel>().Remove(model);
            return true;
        }

        public async Task<IEnumerable<TModel>> GetAll()
        {
            if(typeof(TModel) == typeof(Product))
            {
                var products = await appDbContext.Products.Include(prod=>prod.Category).ToListAsync();
                return products as IEnumerable<TModel>;
            }
            var instance = await appDbContext.Set<TModel>().ToListAsync<TModel>();
            return instance;
        }

        public async Task<TModel> GetById(int id)
        {
            var model = await appDbContext.Set<TModel>().FindAsync(id);
            return model;
        }

        public async Task Update(TModel model)
        {
            appDbContext.Set<TModel>().Update(model);
            appDbContext.SaveChanges();
            throw new NotImplementedException();
        }
    }
}
