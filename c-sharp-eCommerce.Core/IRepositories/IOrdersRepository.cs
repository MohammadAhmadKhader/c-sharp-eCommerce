using c_sharp_eCommerce.Core.Models;

namespace c_sharp_eCommerce.Core.IRepositories
{
	public interface IOrdersRepository : IGenericRepository<Order>
	{
		Task<string?> CancelOrderById(int id);
	}
}
