using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Model;

namespace Catalog.API.Repositories
{
    public interface ICatalogRepository
    {
        Task<CatalogItem> GetAsync(string id);

        Task<IEnumerable<CatalogItem>> GetItemByNameAsync(string name);

        Task AddAsync(CatalogItem job);

        Task<bool> ExistsAsync(string id);

        Task UpdateItemAsync( CatalogItem catalogItem);

        Task DeleteAsync(string id);

        Task<IEnumerable<CatalogItem>> GetItems(int pageSize, int pageIndex);
    }
}