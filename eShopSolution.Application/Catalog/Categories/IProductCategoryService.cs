using eShopSolution.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public interface IProductCategoryService
    {
        Task<int> CreateProductCategory(CategoryCreateViewModel viewModel);
        Task<int> UpdateProductCategory(CategoryUpdateViewModel viewModel);
        Task<bool> DeleteProductCategory(int CategoryId);
        Task<CategorySearchViewModel> GetCategoryById(int Id);
    }
}
