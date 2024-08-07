using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c_shap_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class UnitOfWork<TModel> : IUnitOfWork<TModel> where TModel : class
    {
        private readonly AppDbContext appDbCpntext;
        private readonly IServiceScopeFactory serviceScopeFactory;  
        public UnitOfWork(AppDbContext AppDbContext, IServiceScopeFactory serviceScopeFactory)
        {
            this.appDbCpntext = AppDbContext;
            this.serviceScopeFactory = serviceScopeFactory;
            productRepository = new ProductsRepository(AppDbContext, serviceScopeFactory);
            categoryRepository = new CategoriesRepository(AppDbContext, serviceScopeFactory);
            orderRepository = new OrdersRepository(AppDbContext, serviceScopeFactory);
        }
        public IProductsRepository productRepository { get; set; }
        public ICategoriesRepository categoryRepository { get; set; }
        public IOrdersRepository orderRepository { get; set; }
        public int save() => appDbCpntext.SaveChanges();
        public async Task<int> saveAsync(){
            try
            {
                return await appDbCpntext.SaveChangesAsync();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<IDbContextTransaction> startTransactionAsync()
        {
            var transaction = await appDbCpntext.Database.BeginTransactionAsync();
            return transaction;
        }
    }
}
