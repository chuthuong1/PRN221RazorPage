using WebApplication1.DataAccess.Models;
namespace WebApplication1.DataAccess.Manager
{
    public class OrdersManager
    {
        NorthwindContext context;
        public OrdersManager(NorthwindContext context)
        {
            this.context = context;
        }
        public List<Order> GetOrders()
        {
            return context.Orders.ToList();
        }
        public Order GetOrder(int id)
        {
            return context.Orders.FirstOrDefault(p => p.OrderId == id);
        }

    }
}
