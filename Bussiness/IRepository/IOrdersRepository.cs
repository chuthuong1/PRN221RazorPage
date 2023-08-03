using WebApplication1.DataAccess.Models;

namespace WebApplication1.Bussiness.IRepository
{
    public interface IOrdersRepository
    {
        Task<Order> createtOrdes();
    }
}
