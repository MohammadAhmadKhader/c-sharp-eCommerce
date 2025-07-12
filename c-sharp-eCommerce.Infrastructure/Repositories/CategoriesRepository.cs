using c_shap_eCommerce.Core.IRepositories;
using c_shap_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IServiceScopeFactory serviceScopeFactory;
        public CategoriesRepository(AppDbContext AppDbContext, IServiceScopeFactory serviceScopeFactory) : base(AppDbContext, serviceScopeFactory)
        {
            this.appDbContext = AppDbContext;
            this.serviceScopeFactory = serviceScopeFactory;
        }

    }
}