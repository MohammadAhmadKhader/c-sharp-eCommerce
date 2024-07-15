using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class CategoriesRepository: GenericRepository<Category>, ICategoriesRepository
    {
        private readonly AppDbContext appDbContext;
        public CategoriesRepository(AppDbContext AppDbContext) : base(AppDbContext)
        {
            this.appDbContext = AppDbContext;
        }

    }
}
