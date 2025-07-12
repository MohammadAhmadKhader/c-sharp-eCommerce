using Microsoft.EntityFrameworkCore.Storage;

namespace c_shap_eCommerce.Core.IRepositories
{
    public interface IUnitOfWork<TModel> where TModel : class
    {
        public IProductsRepository productRepository { get; set; }
        public ICategoriesRepository categoryRepository { get; set; }
        public IOrdersRepository orderRepository { get; set; }
        public int save();
        public Task<int> saveAsync();
        Task<IDbContextTransaction> startTransactionAsync();

    }
}