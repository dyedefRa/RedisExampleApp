using Newtonsoft.Json;
using RedisExampleApp.API.Models;
using RedisExampleApp.Cache;
using StackExchange.Redis;

namespace RedisExampleApp.API.Repositories
{
    public class ProductRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey = "productCaches";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;
        public ProductRepositoryWithCacheDecorator(IProductRepository productRepository,
            RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = _redisService.GetDb(2);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            //DB ye ekle.
            var newProduct = await _productRepository.CreateAsync(product);
            //key  yok ise
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            {
                //Cache ekle,
                await LoadToCacheFromDbAsync();
            }
            else
            {
                //Key var ise git cacheye ekle sadece.
                await _cacheRepository.HashSetAsync(productKey, newProduct.Id, JsonConvert.SerializeObject(newProduct));
            }

            return newProduct;
        }

        public async Task<List<Product>> GetAsync()
        {
            //KEYDE YOK ISE 
            if (!await _cacheRepository.KeyExistsAsync(productKey))
                //Db ye git data getir, cachee ekle .
                return await LoadToCacheFromDbAsync();

            var products = new List<Product>();

            //Cacheden al.
            var cachedProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var cachedProduct in cachedProducts)
            {
                var product = JsonConvert.DeserializeObject<Product>(cachedProduct.Value);
                products.Add(product);
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            //YOK ISE
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            {
                // CACHE OLSUTUR VE DONEN DEGERDE ARA
                var products = await LoadToCacheFromDbAsync();
                return products.Find(x => x.Id == id);
            }

            // VAR ISE CACHEDEN AL.
            var productCache = await _cacheRepository.HashGetAsync(productKey, id);
            if (productCache.HasValue)
            {
                return JsonConvert.DeserializeObject<Product>(productCache);
            }

            return null;
        }


        // Db den al cache e yükle ve dataları getir.
        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _productRepository.GetAsync();

            products.ForEach(x =>
            {
                _cacheRepository.HashSetAsync(productKey, x.Id, JsonConvert.SerializeObject(x));
            });

            return products;
        }

    }
}
