
using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities;
using eShopSolution.ViewModels.Catalog;
using eShopSolution.ViewModels.Catalog.Products;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
       private readonly  EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(EShopDbContext context, IStorageService storageService )
        {
            _context = context;
            _storageService = storageService;
        }

        // add list image for a product
        public async Task<int> AddImage(int productId, List<IFormFile> files)
        {
            if (productId == 0) throw new eShopException($"Can not find {productId}");
            var index = 0;
            foreach(var item in files)
            {
                index++;
                var productImage = new ProductImage()
                {
                    ProductId = productId,
                    CreateDate = DateTime.Now,
                    FileSize = item.Length,
                    ImagePath = await SaveFile(item),
                    SortOrder = index
                };
                _context.productImages.Add(productImage);
            }
            return await _context.SaveChangesAsync();
        }

        // add view count
        public async Task<int> AddViewCount(int ProductId)
        {
            var product = await _context.Products.Where(x => x.Id == ProductId).SingleOrDefaultAsync();
            product.ViewCount = product.ViewCount + 1;
            return await _context.SaveChangesAsync();
        }

        //create product
        public async Task<int> CreateProduct(ProductCreateRequest productCreateRequest)
        {
            
            var product = new Product() {
                Price = productCreateRequest.Price,
                OriginalPrice = productCreateRequest.OriginalPrice,
                Stock = productCreateRequest.Stock,
                ViewCount = 0,
                DateCreate = DateTime.Now,
                CategoryId = productCreateRequest.Category,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = productCreateRequest.Name,
                        Description = productCreateRequest.Description,
                        Details = productCreateRequest.Details,
                        SeoAlias = productCreateRequest.SeoAlias,
                        SeoDescription = productCreateRequest.SeoDescription,
                        SeoTitle = productCreateRequest.SeoTitle,
                        LanguageId = productCreateRequest.LanguageId
                    }
                }

            };
            // save image
            if(productCreateRequest.ThumbnailIamge != null)
            {
                product.productImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption="Thumbnail Image",
                        CreateDate = DateTime.Now,
                        FileSize = productCreateRequest.ThumbnailIamge.Length,
                        ImagePath= await SaveFile(productCreateRequest.ThumbnailIamge),
                        IsDefault= true,
                        SortOrder = 1,

                    }
                };
            }
            _context.Products.Add(product);
           
            await _context.SaveChangesAsync();
            if(product.Id !=0)
            {
                await _storageService.SaveFileAsync(productCreateRequest.ThumbnailIamge.OpenReadStream(), product.productImages[0].ImagePath);
            }
            return product.Id;


        }

        // delete product by id
        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.Where(x => x.Id == productId).SingleOrDefaultAsync();
            if (product == null) throw new eShopException($"Can not find product:{productId}");
            List<ProductImage> productImages = await _context.productImages.Where(x => x.ProductId == productId).ToListAsync();
            foreach(var image in productImages)
            {
               await _storageService.DeleteFileAsync(image.ImagePath);
            }
            if(productImages != null)
            {
                _context.RemoveRange(productImages);
            }

            _context.Remove(product);
            return await _context.SaveChangesAsync();
        }

        // get all product
        public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        //Get product and paging
        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
           // select join 
            var query = (from p in _context.Products
                         join pt in _context.ProductTranslations
                         on p.Id equals pt.ProductId
                         select new ProductViewModel() { 
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

            // search theo key word      
            if (request.keyword != null)
            {
                query = query.Where(x => x.Name.Contains(request.keyword));
            }
            // Paging
            var total = await query.CountAsync(); // lấy số lượng record
            var data = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);

            // select and projection
            var pageResult = new PageResult<ProductViewModel> {
                TotalRecord = total,
                Items = await data.ToListAsync()
            };
            return pageResult;

        }

        // get list product image by productId
        public async Task<List<ProductImageViewlMode>> GetListImage(int productId)
        {
            List<ProductImageViewlMode> productImages = await ( from pi in _context.productImages 
                                       where pi.ProductId == productId
                                        select new ProductImageViewlMode() { 
                                            Id = pi.Id,
                                            FilePath= pi.ImagePath,
                                            Caption = pi.Caption,
                                            FileSize = pi.FileSize,
                                            IsDefault = pi.IsDefault
                                       }).ToListAsync();
            return productImages;
        }

        // get product by Id
        public async Task<ProductSearchViewModel> GetProductById(int id )
        {
            var product = await (from p in _context.Products
                                 join pt in _context.ProductTranslations
                                 on p.Id equals pt.ProductId
                                 where p.Id == id
                                 select new ProductSearchViewModel()
                                 {
                                     Id = p.Id,
                                     DateCreated = p.DateCreate,
                                     Description = pt.Description,
                                     Details = pt.Details,
                                     LanguageId = pt.LanguageId,
                                     Name = pt.Name,
                                     OriginalPrice = p.OriginalPrice,
                                     Price = p.Price,
                                     SeoAlias = pt.SeoAlias,
                                     SeoDescription = pt.SeoDescription,
                                     SeoTitle= pt.SeoTitle,
                                     Stock = p.Stock,
                                     ViewCount = p.ViewCount
                                     
                                 }).FirstOrDefaultAsync();
            if (product == null) throw new eShopException($"Can not find product by {id}");
            var productImage = await (_context.productImages.Where(x => x.ProductId == product.Id)).FirstOrDefaultAsync();
            product.imagePath = productImage.ImagePath;
            return product;

            
        }
        // get product by id
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
            if (getProductPagingPublishRequest.keyword != null)
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

        // remove Image by imageId
        public  async Task<int> RemoveImage(int imageId)
        {
            var productImage =await  _context.productImages.Where(x => x.Id == imageId).FirstOrDefaultAsync();
               _context.productImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        // update image by imageId
        public async Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            var productImage = await _context.productImages.Where(x => x.Id == imageId).FirstOrDefaultAsync();
            if (productImage == null) throw new eShopException($"Can not find product by Id {imageId}");
            productImage.Caption = caption;
            productImage.IsDefault = isDefault;
            return await _context.SaveChangesAsync();

        }

        // update price of product
        public async Task<bool> UpdatePrice(int ProductId, decimal newPrice)
        {
            var product = await _context.Products.Where(x => x.Id == ProductId).FirstOrDefaultAsync();
            if (product == null) throw new eShopException($"Can not find product {ProductId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync() > 0;
        }

        // update product
        public async Task<int> UpdateProduct(ProductUpdateRequest productEditRequest)
        {
            var product = await _context.Products.Where(x => x.Id == productEditRequest.Id).FirstOrDefaultAsync();
            var productTranslation = await _context.ProductTranslations
                .Where(x => x.Id == productEditRequest.Id && x.LanguageId == productEditRequest.LanguageId).FirstOrDefaultAsync();
            if (product == null) throw new eShopException($"Can not find product {productEditRequest.Id}");
            productTranslation.Name = productEditRequest.Name;
            productTranslation.SeoAlias = productEditRequest.SeoAlias;
            productTranslation.SeoDescription = productEditRequest.SeoDescription;
            productTranslation.SeoTitle = productEditRequest.SeoTitle;
            productTranslation.Description = productEditRequest.Description;
            productTranslation.Details = productEditRequest.Details;
            product.CategoryId = productEditRequest.Category;
           
            // update image
            if (productEditRequest.ThumbnailIamge != null)
            {
                var thumbnailImage = await _context.productImages.Where(x => x.ProductId == productEditRequest.Id && x.IsDefault == true).FirstOrDefaultAsync();
                if(thumbnailImage != null)
                {
                    thumbnailImage.FileSize = productEditRequest.ThumbnailIamge.Length;
                    thumbnailImage.ImagePath = await SaveFile(productEditRequest.ThumbnailIamge);
                    _context.productImages.Update(thumbnailImage);
                }
               
            }
            return  await _context.SaveChangesAsync();

           
        }

        public async Task<bool> UpdateStock(int ProductId, int addedQuantity)
        {
            var product = await _context.Products.Where(x => x.Id == ProductId).FirstOrDefaultAsync();
            if (product == null) throw new eShopException($"Can not find product {ProductId}");
            product.Stock = addedQuantity;
            return await _context.SaveChangesAsync() > 0;
        }

        // function savefile image
        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
           
            return  fileName;
        }

     
    }
}
