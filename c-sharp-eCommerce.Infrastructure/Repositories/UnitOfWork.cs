using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class UnitOfWork<TModel> : IUnitOfWork<TModel> where TModel : class
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ProductRepository = new ProductsRepository(context);
            CategoryRepository = new CategoriesRepository(context);
            OrderRepository = new OrdersRepository(context);
        }
        public IProductsRepository ProductRepository { get; set; }
        public ICategoriesRepository CategoryRepository { get; set; }
        public IOrdersRepository OrderRepository { get; set; }
        public int Save() => _context.SaveChanges();
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> StartTransactionAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            return transaction;
        }
    }
}
