using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly AppDbContext appDbContext;
        private readonly IServiceScopeFactory serviceScopeFactory;
        public GenericRepository(AppDbContext AppDbContext, IServiceScopeFactory serviceScopeFactory) {
            this.appDbContext = AppDbContext;
            this.serviceScopeFactory = serviceScopeFactory;
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

        public async Task<(IEnumerable<TModel>, int count)> GetAll(int Page,int Limit,string[]? IncludedProperty = null)
        {
            using var scope1 = serviceScopeFactory.CreateScope();
            using var scope2 = serviceScopeFactory.CreateScope();

			var dbContext1 = scope1.ServiceProvider.GetRequiredService<AppDbContext>();
			var dbContext2 = scope2.ServiceProvider.GetRequiredService<AppDbContext>();

            IQueryable<TModel> listQuery = dbContext1.Set<TModel>();
            IQueryable<TModel> countQuery = dbContext2.Set<TModel>();
            
			if (IncludedProperty is not null)
			{
				foreach (var property in IncludedProperty)
				{
					listQuery = listQuery.Include(property);
				}
			}

            var listTask = listQuery.OrderBy(x => x).Skip((Page - 1) * Limit).Take(Limit).ToListAsync();
            var countTask = countQuery.CountAsync();

			await Task.WhenAll(listTask, countTask);

			var list = await listTask;
			var count = await countTask;

            return (list, count);
        }

        public async Task<TModel> GetById(int id)
        {
            var resource = await appDbContext.Set<TModel>().FindAsync(id) ;
            return resource;
        }

        public async Task Update(object PK,Action<TModel> UpdateResource)
        {
            try
            {
                var resource = await appDbContext.Set<TModel>().FindAsync(PK);
                if (resource is null)
                {
                    throw new ArgumentException($"{typeof(TModel).Name} was not found.");
                }

                UpdateResource(resource);
            }catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
