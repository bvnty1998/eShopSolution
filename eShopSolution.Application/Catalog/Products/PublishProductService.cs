
using eShopSolution.Data.EF;
using eShopSolution.ViewModels.Catalog.Products;

using eShopSolution.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class PublishProductService : IPublishProductService
    {
        public readonly EShopDbContext _context;
        public PublishProductService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<PageResult<ProductViewModel>> GetpProductByCategory(GetProductPagingPublishRequest getProductPagingPublishRequest)
        {
  
            // select join 
           var query = (from p in _context.Products
                        join pt in _context.ProductTranslations
                        on p.Id equals pt.ProductId
                        select new ProductViewModel()
                        {
                            Id = p.Id,
                            Name = pt.Name,
                            Description = pt.Description,
                            Details = pt.Details,
                            DateCreate = p.DateCreate,
                            LanguageId = pt.LanguageId,
                            OriginalPrice = p.OriginalPrice,
                            Price = p.Price,
                            SeoAlias = pt.SeoAlias,
                            SeoDescription = pt.SeoDescription,
                            SeoTitle = pt.SeoTitle,
                            Stock = p.Stock,
                            ViewCount = p.ViewCount

                        });
            // search theo key work
            if(getProductPagingPublishRequest.keyword != null)
            {
                query = query.Where(x => x.Name.Contains(getProductPagingPublishRequest.keyword));
            }
            // total record
            var total = await query.CountAsync();
            PageResult<ProductViewModel> pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = total,
                Items = await query.Skip((getProductPagingPublishRequest.PageIndex - 1) * getProductPagingPublishRequest.PageSize)
                .Take(getProductPagingPublishRequest.PageSize).ToListAsync()
            };
            return pageResult;      
        }
    }
}
