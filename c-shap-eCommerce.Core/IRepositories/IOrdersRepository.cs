using c_shap_eCommerce.Core.Models;

namespace c_shap_eCommerce.Core.IRepositories
{
    public interface IOrdersRepository : IGenericRepository<Order>
    {
        Task<string?> CancelOrderById(int Id);
    }
}