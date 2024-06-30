using c_shap_eCommerce.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        public void Create(TModel model)
        {
            Console.WriteLine("Create");
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            Console.WriteLine("Delete");
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> GetAll()
        {
            Console.WriteLine("Get All");
            throw new NotImplementedException();
        }

        public int GetById(int id)
        {
            Console.WriteLine("Get By Id");
            throw new NotImplementedException();
        }

        public void Update(TModel model)
        {
            Console.WriteLine("Update");
            throw new NotImplementedException();
        }
    }
}
