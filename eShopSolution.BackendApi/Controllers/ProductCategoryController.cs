using eShopSolution.Application.Catalog.Categories;
using eShopSolution.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductCategoryController : Controller
    {
        IProductCategoryService _category;
        public ProductCategoryController(IProductCategoryService category)
        {
            _category = category;
        }
        [HttpPost("CreateCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel viewModel)
        {
            var result = await _category.CreateProductCategory(viewModel);
            var category = await _category.GetCategoryById(result);
            return new OkObjectResult(category);
        }
        [HttpPut("UpdateCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromForm] CategoryUpdateViewModel viewModel)
        {
            var result = await _category.UpdateProductCategory(viewModel);
            var category = await _category.GetCategoryById(result);
            return new OkObjectResult(category);
        }
        [HttpDelete("DeleteCategory/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _category.DeleteProductCategory(id);
            if(result == false)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkObjectResult(new { 
                  status="success",
                  Message="Delete sucessfully !!"
                });
            }    
        }
    }
}
