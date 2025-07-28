using Microsoft.EntityFrameworkCore.Storage;

namespace c_sharp_eCommerce.Core.IRepositories
{
    public interface IUnitOfWork<TModel> where TModel : class
    {
        public IProductsRepository ProductRepository { get; set; }
        public ICategoriesRepository CategoryRepository { get; set; }
        public IOrdersRepository OrderRepository { get; set; }
        public int Save();
        public Task<int> SaveAsync();
        Task<IDbContextTransaction> StartTransactionAsync();
    }
}
