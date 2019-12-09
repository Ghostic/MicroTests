using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.IntegrationEvents.Events;
using Catalog.API.Model;
using Catalog.API.Repositories;
using Common.EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly IEventBus _eventBus;

        public CatalogController(ICatalogRepository catalogRepository, IEventBus eventBus)
        {
            _catalogRepository = catalogRepository;
            _eventBus = eventBus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<CatalogItem>>> ItemsAsync([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            var items = await _catalogRepository.GetItems(pageSize, pageIndex);

            return Ok(items);
        }

        [HttpGet]
        [Route("id/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CatalogItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CatalogItem>> ItemByIdAsync(string id)
        {
            var job = await _catalogRepository.GetAsync(id);

            if(job != null)
            {
                return Ok(job);                 
            }

            return NotFound();
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<ActionResult> ItemByName(string name)
        {
            var items = await _catalogRepository.GetItemByNameAsync(name);        
            return Ok(items);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]    
        public async Task<ActionResult> CreateProductAsync(CatalogItem product)
        {
            var newProduct = new CatalogItem
            {
                Description = product.Description,
                Name = product.Name,
                Price = product.Price
            };
            await _catalogRepository.AddAsync(newProduct);

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = newProduct.Id}, null);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateProductAsync([FromBody]CatalogItem productToUpdate)
        {
            var catalogItem = await _catalogRepository.GetAsync( productToUpdate.Id );

            if(catalogItem == null)
            {
                return NotFound(new {Message = $"Item with id {productToUpdate.Id} not found."});
            }

            var oldPrice = catalogItem.Price;
            var raiseProductPriceChangedEvent = oldPrice != productToUpdate.Price;

            catalogItem = productToUpdate;       

            if(raiseProductPriceChangedEvent)
            {

                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(catalogItem.Id, productToUpdate.Price, oldPrice);
                _eventBus.Publish(priceChangedEvent);
                await _catalogRepository.UpdateItemAsync(catalogItem);
            }
            else
            {
                await _catalogRepository.UpdateItemAsync(catalogItem);
            }

            return CreatedAtAction(nameof(ItemByIdAsync),  new { id = productToUpdate.Id }, null);
        }

        [HttpDelete]
        [Route("id/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteProductAsync(string id)
        {
            var job = _catalogRepository.GetAsync(id);
            if(job  == null)
            {
                return NotFound();
            }

            await _catalogRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}