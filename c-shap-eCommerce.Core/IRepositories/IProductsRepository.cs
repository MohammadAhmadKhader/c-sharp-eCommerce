using c_shap_eCommerce.Core.Models;

namespace c_shap_eCommerce.Core.IRepositories
{
    public interface IProductsRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryId(int CategoryId, int page, int limit);
        Task<Product> GetProductById(int Id);
        Task<List<Product>> GetProductsByIds(List<int> ListOfIds);

    }
}