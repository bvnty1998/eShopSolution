
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using System;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
   public interface IPublishProductService
    {
        Task<PageResult<ProductViewModel>> GetpProductByCategory(GetProductPagingPublishRequest getProductPagingPublishRequest);
       
    }
}
