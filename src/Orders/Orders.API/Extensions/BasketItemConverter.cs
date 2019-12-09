using System.Collections.Generic;
using Orders.API.Model;

namespace Orders.API.Extensions
{
    public static class BasketItemConverter
    {
        public static IEnumerable<OrderItemDTO> ToOrderItemsDTO(this IEnumerable<BasketItem> basketItems)
        {
            foreach (var item in basketItems)
            {
                yield return item.ToOrderItemDTO();
            }
        }

        public static OrderItemDTO ToOrderItemDTO(this BasketItem item)
        {
            return new OrderItemDTO()
            {
                ProductId = int.TryParse(item.ProductId, out int id) ? id : -1,
                ProductName = item.ProductName,
                UnitPrice = item.UnitPrice,
                Units = item.Quantity
            };
        }
    }
}