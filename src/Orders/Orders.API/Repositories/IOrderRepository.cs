using System.Threading.Tasks;
using Orders.API.Model;

namespace Orders.API.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        
        Task UpdateAsync(Order order);

        Task<Order> GetAsync(string orderId);
    }
}