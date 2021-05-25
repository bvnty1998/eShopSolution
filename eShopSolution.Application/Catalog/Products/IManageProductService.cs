
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
   public interface IManageProductService
    {
        Task<int> CreateProduct(ProductCreateRequest productCreateRequest);
        Task<int> UpdateProduct(ProductUpdateRequest productEditRequest);
        Task<bool> UpdatePrice(int ProductId, decimal newPrice);
        Task<bool> UpdateStock(int ProductId, int addedQuantity);
        Task<int> AddViewCount(int ProductId);
        Task<int> Delete(int productId);
        Task<List<ProductViewModel>> GetAll();
        Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request);
        Task<ProductSearchViewModel> GetProductById(int id);
        Task<int> AddImage(int productId, List<IFormFile> files);
        Task<int> RemoveImage(int imageId);
        Task<int> UpdateImage(int imageId, string caption,bool isDefault);
        Task<List<ProductImageViewlMode>> GetListImage(int productId);
    }
}
