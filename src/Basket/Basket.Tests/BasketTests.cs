using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.API.Controllers;
using Basket.API.Model;
using Basket.API.Repository;
using Common.EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Basket.Tests
{
    public class BasketTests
    {
        private readonly Mock<IBasketRepository> _basketRepositoryMock;
        private readonly Mock<IEventBus> _serviceBusMock;
        private readonly Mock<ILogger<BasketController>> _loggerMock;

        public BasketTests()
        {
            _basketRepositoryMock = new Mock<IBasketRepository>();
            _serviceBusMock = new Mock<IEventBus>();
            _loggerMock = new Mock<ILogger<BasketController>>();
        }

        [Fact]
        public async Task Get_Customer_Basket_Success()
        {
            // Arrange
            var fakeId = "5d87b0b19281994590e08551";
            var fakeBasket = GetFakeBasket();
            _basketRepositoryMock.Setup(x => x.GetBasketAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeBasket));
           
           // Act
            var basketController = new BasketController(
                _loggerMock.Object,
                _basketRepositoryMock.Object,
                _serviceBusMock.Object
            );
          
            var actionResult = await basketController.GetBasketByIdAsync(fakeId);

            // Assert
            Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal((((ObjectResult)actionResult.Result).Value as CustomerBasket).BuyerId, fakeId);           
        }

        [Fact]
        public async Task Post_Customer_Basket_Success()
        {
            // Arrange
            var userId = "5d87b0b19281994590e08551";
            var fakeBasket = GetFakeBasket();
            _basketRepositoryMock.Setup(x => x.UpdateBasketAsync(It.IsAny<CustomerBasket>())).Returns(Task.FromResult(fakeBasket));
      
            // Act
            var basketController = new BasketController(
                _loggerMock.Object,
                _basketRepositoryMock.Object,
                _serviceBusMock.Object
            );          
            var actionResult = await basketController.UpdateBasketAsync(fakeBasket);

            // Assert
            Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
            Assert.Equal((((ObjectResult)actionResult.Result).Value as CustomerBasket).BuyerId, userId);   
        }

        private CustomerBasket GetFakeBasket()
        {
            var userId = "5d87b0b19281994590e08551";
            return new CustomerBasket(userId)
            {
                Items = new List<BasketItem>()
                {
                    new BasketItem()
                }
            };
        }
    }
}
