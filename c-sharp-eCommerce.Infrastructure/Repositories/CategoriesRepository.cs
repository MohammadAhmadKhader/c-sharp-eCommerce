using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Infrastructure.Data;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class CategoriesRepository : GenericRepository<Category>, ICategoriesRepository
    {
        private readonly AppDbContext _context;
        public CategoriesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
