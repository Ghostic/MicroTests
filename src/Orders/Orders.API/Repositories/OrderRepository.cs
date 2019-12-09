using System.Threading.Tasks;
using Common.Mongo;
using Orders.API.Model;

namespace Orders.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoRepository<Order> repository;

        public OrderRepository(IMongoRepository<Order> repository)
        {
            this.repository = repository;
        }

        public async Task AddAsync(Order order)
            => await repository.AddAsync(order);
            
        public async Task<Order> GetAsync(string orderId)
            => await repository.GetAsync(orderId);

        public async Task UpdateAsync(Order order)
            => await repository.UpdateAsync(order);
    }
}