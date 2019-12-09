using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Controllers;
using Catalog.API.IntegrationEvents.Events;
using Catalog.API.Model;
using Catalog.API.Repositories;
using Common.EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.Tests
{
    public class CatalogTests
    {
        private readonly Mock<ICatalogRepository> _catalogRepositoryMock;
        private readonly Mock<IEventBus> _serviceBusMock;

        public CatalogTests()
        {
            _catalogRepositoryMock = new Mock<ICatalogRepository>();
            _serviceBusMock = new Mock<IEventBus>();
        }

        [Fact]
        public async Task Get_Catalog_Items_Success()
        {
            // Arrange
            var fakeCatalogItems = GetFakeCatalog();
            _catalogRepositoryMock.Setup(x => x.GetItems(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(fakeCatalogItems));

            // Act
            var catalogController = new CatalogController(_catalogRepositoryMock.Object, _serviceBusMock.Object);
            var actionResult = await catalogController.ItemsAsync();

            // Assert
            Assert.Equal((actionResult.Result as OkObjectResult).StatusCode, (int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Update_Product_With_New_Price_Should_Publish_ProductPriceChangeIntegrationEvent()
        {
            // Arrange
            var fakeCatalogItem = new CatalogItem{ Id = "5de940c8a7513a147ab5feab", Name = "Catalog1", Price = 3.00m};
            var fakePostCatalog = new CatalogItem{ Id = "5de940c8a7513a147ab5feab", Name = "Catalog1", Price = 5.00m};

            _catalogRepositoryMock.Setup(x => x.GetAsync(It.IsAny<string>())).Returns(Task.FromResult(fakeCatalogItem));

            // Act
            var catalogController = new CatalogController(_catalogRepositoryMock.Object, _serviceBusMock.Object);          
            var result = await catalogController.UpdateProductAsync(fakePostCatalog);

            // Assert
            _serviceBusMock.Verify(mock => mock.Publish(It.IsAny<ProductPriceChangedIntegrationEvent>()), Times.Once);
            Assert.Equal((result as ObjectResult).StatusCode, (int)HttpStatusCode.Created);         
        }

        private IEnumerable<CatalogItem> GetFakeCatalog()
        {
            return new List<CatalogItem>
            {
                new CatalogItem{ Id = "5de940c8a7513a147ab5feab", Name = "Catalog1", Price = 3.00m},
                new CatalogItem{ Id = "5de940e4b08a708d6938414a", Name = "Catalog2", Price = 3.00m}
            };
        }
    }
}
