using eShopSolution.ViewModels.Catalog.ProductImage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.ProductImages
{
    public interface IProductImageService
    {
        Task<int> CreateProducutImage(ProductImageCreateViewModel viewModel);
        Task<int> UpdateProducutImage(ProductImageUpdateViewModel viewModel);
        Task<ProductImageSearchViewModel> GetProductImageById(int Id);
    }
}
