using c_shap_eCommerce.Core.Models;
using c_shap_eCommerce.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using c_sharp_eCommerce.Infrastructure.Data;

namespace c_sharp_eCommerce.Infrastructure.Repositories
{
    public class OrdersRepository : GenericRepository<Order>, IOrdersRepository
    {
        private readonly AppDbContext appDbContext;
        public OrdersRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<string?> CancelOrderById(int Id)
        {
            string? errMessage;
            var order = await appDbContext.Orders.FindAsync(Id);
            if(order == null)
            {
				errMessage = "NotFound";
				return errMessage;
			}
            if(order.Status == "cancelled")
            {
				errMessage = "AlreadyCancelled";
				return errMessage;
			}
            order.Status = "cancelled";

            return null;
        }
    }
}
