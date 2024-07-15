using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c_shap_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Infrastructure.Data;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class UnitOfWork<TModel> : IUnitOfWork<TModel> where TModel : class
    {
        private readonly AppDbContext appDbCpntext;
        public UnitOfWork(AppDbContext AppDbContext)
        {
            this.appDbCpntext = AppDbContext;
            productRepository = new ProductsRepository(AppDbContext);
            categoryRepository = new CategoriesRepository(AppDbContext);
        }
        public IProductsRepository productRepository { get; set; }
        public ICategoriesRepository categoryRepository { get; set; }
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
    }
}
