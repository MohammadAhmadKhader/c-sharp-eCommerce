using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.IRepositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        public IEnumerable<TModel> GetAll();
        public int GetById(int id);
        public void Create(TModel model);
        public void Update(TModel model);
        public void Delete(int id);
    }
}
