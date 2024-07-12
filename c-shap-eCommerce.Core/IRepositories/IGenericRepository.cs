using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.IRepositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        public Task<IEnumerable<TModel>> GetAll();
        public Task<TModel> GetById(int id);
        //public void Create(TModel model);
        //public void Create(TModel model);
        public Task Create(TModel model);
        public Task Update(TModel model);
        public Task<bool> Delete(int id);
    }
}
