using c_shap_eCommerce.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.IRepositories
{
    public interface IProductsRepository : IGenericRepository<Product>
    {
        public Task<IEnumerable<Product>> GetProductsByCategoryId(int CategoryId);
    }
}
