using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Model;
using Common.Mongo;
namespace Catalog.API.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly IMongoRepository<CatalogItem> repository;

        public CatalogRepository(IMongoRepository<CatalogItem> repository)
        {
            this.repository = repository;
        }

        public async Task<CatalogItem> GetAsync(string id)
            => await repository.GetAsync(id);

        public async Task<IEnumerable<CatalogItem>> GetItemByNameAsync(string name)
            => await repository.FindAsync(x => x.Name == name);

        public async Task AddAsync(CatalogItem job)
            => await repository.AddAsync(job);

        public async Task<bool> ExistsAsync(string id)
            => await repository.ExistsAsync(i => i.Id == id);

        public async Task UpdateItemAsync(CatalogItem catalogItem)
            => await repository.UpdateAsync(catalogItem);

        public async Task DeleteAsync(string id)
            => await repository.DeleteAsync(id);

        public async Task<IEnumerable<CatalogItem>> GetItems(int pageSize, int pageIndex)
            => await repository.FindPage(pageSize, pageIndex);
    }
}