using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublishProductService _publishProduct;
        private readonly IManageProductService _manageProduct;
        public ProductController(IPublishProductService publishProduct,IManageProductService manageProduct)
        {
            _publishProduct = publishProduct;
            _manageProduct = manageProduct; 
        }

        #region product publish
        // http://localhost:port/product/public-paging
        [HttpGet("public-paging")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromForm]GetProductPagingPublishRequest request)
        {
            var result = await _publishProduct.GetpProductByCategory(request);
            return new OkObjectResult(result);
        }
        #endregion

        #region product Manage
        // http://localhost:port/product/CreateProduct
        [HttpPost("CreateProduct")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest request)
        {
            var result = await _manageProduct.CreateProduct(request);
           if(result == 0)
            {
                return BadRequest();
            }
           else
            {
                return new OkObjectResult(await _manageProduct.GetProductById(result));
            }
        }

        // http://localhost:port/product/UpdateProduct
        [HttpPost("UpdateProduct")]
        [AllowAnonymous]
        public async Task<IActionResult> Put([FromForm] ProductUpdateRequest request)
        {
            var result = await _manageProduct.UpdateProduct(request);
            return new OkObjectResult(result);
        }
        // http://localhost:port/product/Delete/{productId}
        [HttpDelete("DeleteProduct/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int productId)
        {
            var result = await _manageProduct.Delete(productId);
            return new OkObjectResult(result);  
        }

        // http://localhost:port/product/Price/{id}/{newPrice}
        [HttpPut("Price")]
        public async Task<IActionResult> UpdatePrice([FromForm] int id, [FromForm] decimal newPrice)
        {
            var result =await _manageProduct.UpdatePrice(id, newPrice);
            return new OkObjectResult(result);
        }

        // http://localhost:port/product/Stock/{id}/{addedQuantity}
        [HttpPut("Stock")]
        public async Task<IActionResult> UpdateStock([FromForm] int id, [FromForm] int addedQuantity)
        {
            var result = await _manageProduct.UpdateStock(id, addedQuantity);
            return new OkObjectResult(result);
        }
        // http://localhost:portProduct/GetProductById/{id}}
        [HttpGet("GetProductById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _manageProduct.GetProductById(id);
            if(result == null)
            {
                return NotFound();
            }
            return new OkObjectResult(result);
        }

        #endregion

    }
}
