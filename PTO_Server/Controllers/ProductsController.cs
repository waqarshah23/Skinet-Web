using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.Extensions.Caching.Memory;
using PTO_Server.Extensions.Logger;
using Core.Models;
using PTO_Server.Repository;
using Infrastructure.Cache;
using Core.Specifications;
using PTO_Server.Dto;
using AutoMapper;

namespace PTO_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private const string productList = "productsList";
        private readonly IRepository<Products> _Productrepository;
        private readonly IRepository<ProductBrand> _Brandrepository;
        private readonly IRepository<type> _Typerepository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private IMemoryCache _cache;
        public ProductsController(IRepository<Products> Productrepository,
            IRepository<ProductBrand> Brandrepository,
            IRepository<type> Typerepository, ILoggerManager logger,
            IMapper mapper, IMemoryCache cache )
        {
            _Productrepository = Productrepository;
            _Brandrepository = Brandrepository;
            _Typerepository = Typerepository;
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsToReturnDto>>> getAllProducts()
        {
            var specs = new ProductsWithTypeAndBrandSpecification();
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
                    productsList = await _Productrepository.ListWithSpecAsync(specs);
                    if(productsList == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        _setUserListCache(productsList);
                    }
                }
                return Ok(_mapper.Map<IEnumerable<Products>, IEnumerable<ProductsToReturnDto>>(productsList));
                //return Ok(productsList.Select(product => new ProductsToReturnDto
                //{
                //    Id = product.Id,
                //    Name = product.Name,
                //    Description = product.Description,
                //    Price = product.Price,
                //    PictureUrl = product.PictureUrl ,
                //    ProductBrand = product.ProductBrand.name,
                //    ProductType = product.ProductType.name
                //}).ToList());
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsToReturnDto>> getProductById(int id)
        {
            var specs = new ProductsWithTypeAndBrandSpecification(id);
            try
            {
                _logger.LogInfo("getting product by ID");
                //Products Item = Cache.getItemFromCache<Products>("productbyId", await _getProductByID(id));
                Products product = await _Productrepository.GetEntityWithSpec(specs);
                if(product == null)
                {
                    _logger.LogInfo("product not found");
                    return NotFound(id);
                }
                return Ok(
                    _mapper.Map<Products, ProductsToReturnDto>(product)
                    );
                //return Ok(new ProductsToReturnDto
                //{
                //    Id = product.Id,
                //    Name = product.Name,
                //    Description = product.Description,
                //    Price = product.Price,
                //    PictureUrl = product.PictureUrl,
                //    ProductBrand = product.ProductBrand.name == null ? "" : product.ProductBrand.name,
                //    ProductType = product.ProductType.name == null ? "" : product.ProductType.name
                //});
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e);
            }
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> getProductBrands()
        {
            IEnumerable<ProductBrand> list = await _Brandrepository.GetListAsync();
            return Ok(new
            {
                ProductBrands = list
            });
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<type>>> getProductTypes()
        {
            IEnumerable<type> list = await _Typerepository.GetListAsync();
            return Ok(new
            {
                ProductTypes = list
            });
        }
        private async Task<Products> _getProductByID(int id)
        {
            Products item = await _Productrepository.GetById(id);
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
