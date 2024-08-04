using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_shap_eCommerce.Core.IRepositories
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        public Task<IEnumerable<TModel>> GetAll(int Page,int Limit, string[]? IncludedProperty = null);
        public Task<TModel> GetById(int id);
        public Task Create(TModel model);
        public Task Update(object PK, Action<TModel> UpdateResource);
        public Task<bool> Delete(int id);
    }
}
