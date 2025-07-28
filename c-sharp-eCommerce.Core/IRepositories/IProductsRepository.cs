using c_sharp_eCommerce.Core.Models;

namespace c_sharp_eCommerce.Core.IRepositories
{
    public interface IProductsRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryId(int categoryId, int page, int limit);
        Task<Product?> GetProductById(int id);
        Task<List<Product>> GetProductsByIds(List<int> listOfIds);

    }
}
