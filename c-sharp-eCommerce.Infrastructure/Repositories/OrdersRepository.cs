using c_sharp_eCommerce.Core.Models;
using c_sharp_eCommerce.Core.IRepositories;
using c_sharp_eCommerce.Infrastructure.Data;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class OrdersRepository : GenericRepository<Order>, IOrdersRepository
    {
        private readonly AppDbContext _context;

        public OrdersRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<string?> CancelOrderById(int id)
        {
            string? errMessage;
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                errMessage = "NotFound";
                return errMessage;
            }
            if (order.Status == "completed")
            {
                errMessage = "order is completed";
                return errMessage;
            }
            if (order.Status == "cancelled")
            {
                errMessage = "AlreadyCancelled";
                return errMessage;
            }
            order.Status = "cancelled";

            return null;
        }
    }
}
