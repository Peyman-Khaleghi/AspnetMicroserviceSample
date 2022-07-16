using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region field

        private readonly ICatalogContext _catalogContext;

        #endregion

        #region ctor

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        #endregion

        #region Method

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext
                          .Products
                          .Find(p => true)
                          .ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _catalogContext
                         .Products
                         .Find(p => p.Id == id)
                         .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            return await _catalogContext
                        .Products
                        .Find(filter)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _catalogContext
                         .Products
                         .Find(filter)
                         .ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext
                 .Products
                 .InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            ReplaceOneResult replaceResult = await _catalogContext
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return replaceResult.IsAcknowledged
                  && replaceResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _catalogContext
                                             .Products
                                             .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                 && deleteResult.DeletedCount > 0;
        }

        #endregion
    }
}
