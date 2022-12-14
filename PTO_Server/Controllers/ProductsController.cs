using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.Extensions.Caching.Memory;
using PTO_Server.Extensions.Logger;
using Core.Models;
using PTO_Server.Repository;
using Infrastructure.Cache;

namespace PTO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private const string productList = "productsList";
        private readonly IRepository<Products> _repository;
        private readonly ILoggerManager _logger;
        private IMemoryCache _cache;
        public ProductsController(IRepository<Products> repository, ILoggerManager logger, IMemoryCache cache )
        {
            _repository = repository;
            _logger = logger;
            _cache = cache;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> getAllProducts()
        {
            IEnumerable<Products> productsList;
            try
            {
                if(_cache.TryGetValue(productList, out productsList))
                {
                    _logger.LogInfo("products List found in cache");
                }
                else
                {
                    _logger.LogInfo("fetching Products List from the Database");
                    productsList = await _repository.GetListAsync();
                    if(productsList == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        _setUserListCache(productsList);
                    }
                }
                return Ok(new
                {
                    Products = productsList
                });
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> getProductById(int id)
        {
            try
            {
                _logger.LogInfo("getting product by ID");
                //Products Item = Cache.getItemFromCache<Products>("productbyId", await _getProductByID(id));
                Products product = await _repository.GetById(id);
                if(product == null)
                {
                    _logger.LogInfo("product not found");
                    return NotFound(id);
                }
                return Ok(product);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e);
            }
        }
        private async Task<Products> _getProductByID(int id)
        {
            Products item = await _repository.GetById(id);
            return item;
        }
        private void _setUserListCache(IEnumerable<Products> pList)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);

            _cache.Set(productList, pList, cacheEntryOptions);
        }
    }
}
