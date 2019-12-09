using System;
using System.Collections.Generic;
using System.Linq;
using Common.Types;
using Orders.API.Extensions;

namespace Orders.API.Model
{
    public class Order : BaseEntity
    {
        private readonly List<OrderItemDTO> _orderItems;
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public string PostCode { get; private set; }
        public string CardNumber { get; private set; }
        public string CardHolderName { get; private set; }
        public DateTime CardExpiration { get; private set; }
        public string CardSecurityNumber { get; private set; }
        public int CardTypeId { get; private set; }
        public IEnumerable<OrderItemDTO> OrderItems => _orderItems;

        public Order()
        {
            _orderItems = new List<OrderItemDTO>();
        }

        public Order(List<BasketItem> basketItems, string userId, string userName, string city, string country,
            string postCode, string cardNumber, string cardHolderName, DateTime cardExpiration,
            string cardSecurityNumber, int cardTypeId) : this()
            {
                _orderItems = basketItems.ToOrderItemsDTO().ToList();
                UserId = userId;
                UserName = userName;
                City = city;
                Country = country;
                PostCode = postCode;
                CardNumber = cardNumber;
                CardHolderName = cardHolderName;
                CardExpiration = cardExpiration;
                CardSecurityNumber = cardSecurityNumber;
                CardTypeId = cardTypeId;
            }
    }

    public class OrderItemDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public int Units { get; set; }

        public string PictureUrl { get; set; }
    }

    
}