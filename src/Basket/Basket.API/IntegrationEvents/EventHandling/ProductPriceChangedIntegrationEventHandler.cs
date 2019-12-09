using System.Linq;
using System.Threading.Tasks;
using Basket.API.IntegrationEvents.Events;
using Basket.API.Model;
using Basket.API.Repository;
using Common.EventBus.Abstractions;
using Microsoft.Extensions.Logging;

namespace Basket.API.IntegrationEvents.EventHandling
{
    public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>
    {
        public ILogger<ProductPriceChangedIntegrationEventHandler> _logger { get; }
        public IBasketRepository _repository { get; }
        public ProductPriceChangedIntegrationEventHandler(ILogger<ProductPriceChangedIntegrationEventHandler> logger, IBasketRepository repository)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(ProductPriceChangedIntegrationEvent @event)
        {
            _logger.LogInformation("----- Handling integration event: {IndegrationEventId} at {AppName} - ({@IntegrationEvent})",
                                    @event.Id, Program.AppName, @event);

            var userIds = _repository.GetUsers();

            foreach( var id in userIds)
            {
                var basket = await _repository.GetBasketAsync(id);

                 await UpdatePriceInBasketItems(@event.ProductId, @event.NewPrice, @event.OldPrice, basket);
            }
        }

        private async Task UpdatePriceInBasketItems(string productId, decimal newPrice, decimal oldPrice, CustomerBasket basket)
        {
            string match = productId.ToString();
            var itemsToUpdate = basket?.Items?.Where(x => x.ProductId == match).ToList();

            if (itemsToUpdate != null)
            {
                _logger.LogInformation("----- ProductPriceChangedIntegrationEventHandler - Updating items in basket for user: {BuyerId} ({@Items})", basket.BuyerId, itemsToUpdate);

                foreach (var item in itemsToUpdate)
                {
                    if (item.UnitPrice == oldPrice)
                    {
                        var originalPrice = item.UnitPrice;
                        item.UnitPrice = newPrice;
                        item.OldUnitPrice = originalPrice;
                    }
                }
                await _repository.UpdateBasketAsync(basket);
            }
        }
    }
}