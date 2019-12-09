using System;
using Common.Types;

namespace Catalog.API.Model
{
    public class CatalogItem : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

    }
}