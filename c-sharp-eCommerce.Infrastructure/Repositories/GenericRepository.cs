using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(TModel model)
        {
            await _context.Set<TModel>().AddAsync(model);
        }

        public async Task<bool> Delete(int id)
        {
            var model = await _context.Set<TModel>().FindAsync(id);
            if (model is null)
            {
                return false;
            }

            _context.Set<TModel>().Remove(model);

            return true;
        }

        public async Task<(IEnumerable<TModel>, int count)> GetAll(int page, int limit, string[]? includedProperties = null)
        {
            IQueryable<TModel> baseQuery = _context.Set<TModel>();
            if (includedProperties != null)
            {
                foreach (var prop in includedProperties)
                {
                    baseQuery = baseQuery.Include(prop);
                }
            }

            var count = await baseQuery.CountAsync();
            var list = await baseQuery
                .OrderBy(x => x)
                .Skip((page - 1) * limit)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();

            return (list, count);
        }

        public async Task<TModel?> GetById(int id)
        {
            var resource = await _context.Set<TModel>().FindAsync(id);
            return resource;
        }

        public async Task Update(object pk, Action<TModel> updateResource)
        {
            try
            {
                var resource = await _context.Set<TModel>().FindAsync(pk);
                if (resource is null)
                {
                    throw new ArgumentException($"{typeof(TModel).Name} was not found.");
                }

                updateResource(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
