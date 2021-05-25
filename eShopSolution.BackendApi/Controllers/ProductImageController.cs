using eShopSolution.Application.Catalog.ProductImages;
using eShopSolution.ViewModels.Catalog.ProductImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : Controller
    {
        public readonly IProductImageService _productImage;
        public ProductImageController(IProductImageService productImage)
        {
            _productImage = productImage;
        }
        
        [HttpPost("CreateProductImage")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] ProductImageCreateViewModel viewModel)
        {
            var result = await _productImage.CreateProducutImage(viewModel);
            if(result ==0)
            {
                return BadRequest();
            }
            var productImage = await _productImage.GetProductImageById(result);
            return new OkObjectResult(productImage);
        }
        [HttpPut("ProductImage/UpdateProductImage")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromForm] ProductImageUpdateViewModel viewModel)
        {
            
              var result = await  _productImage.UpdateProducutImage(viewModel);
            

            if(result == 0 )
            {
                return BadRequest();
            }
            else
            {
                var productImage = _productImage.GetProductImageById(viewModel.Id);
                return new OkObjectResult(productImage);
            }
        }
        [HttpGet("GetProductImageById")]
        [Authorize]
        public async Task<IActionResult> GetProductImageById(int id)
        {
            var reslut =  await _productImage.GetProductImageById(id);
            if(reslut == null)
            {
                return BadRequest(new { 
                status="Faild",
                Message="Cand not find productImage by Id " + id

                });
            }
            else
            {
                return new OkObjectResult(reslut);
            }
        }
    }
}
