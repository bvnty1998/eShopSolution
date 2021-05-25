using eShopSolution.Application.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities;
using eShopSolution.ViewModels.Catalog.ProductImage;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.ProductImages
{
    public class ProductImageService : IProductImageService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        public ProductImageService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }
        public async Task<int> CreateProducutImage(ProductImageCreateViewModel viewModel)
        {
            var productImage = new ProductImage()
            {
                Caption = viewModel.Caption,
                CreateDate = DateTime.Now,
                FileSize = viewModel.ImageFile.Length,
                SortOrder = viewModel.SortOrder,
                ProductId = viewModel.ProductId,
                ImagePath = await SaveFile(viewModel.ImageFile)
            };
            _context.productImages.Add(productImage);
            await _context.SaveChangesAsync();
           if( productImage.Id != 0 )
            {
                await _storageService.SaveFileAsync(viewModel.ImageFile.OpenReadStream(), productImage.ImagePath);
            }
            return productImage.Id;

        }

        public async Task<ProductImageSearchViewModel> GetProductImageById(int Id)
        {
            var productImage = await (  from pi in _context.productImages
                                where pi.Id == Id
                                select new ProductImageSearchViewModel()
                                {
                                    Id = pi.Id,
                                    Caption = pi.Caption,
                                    DateCreated = pi.CreateDate,
                                    FileSize = pi.FileSize,
                                    ImagePath = pi.ImagePath,
                                    IsDefault = pi.IsDefault,
                                    ProductId = pi.ProductId,
                                    SortOrder = pi.SortOrder
                                }).FirstOrDefaultAsync();
           

            return productImage;
        }

        public async Task<int> UpdateProducutImage(ProductImageUpdateViewModel viewModel)
        {
            var productImage = await(from pi in _context.productImages
                                     where pi.Id == viewModel.Id
                                     select new ProductImageSearchViewModel()
                                     {
                                         Id = pi.Id,
                                         Caption = pi.Caption,
                                         DateCreated = pi.CreateDate,
                                         FileSize = pi.FileSize,
                                         ImagePath = pi.ImagePath,
                                         IsDefault = pi.IsDefault,
                                         ProductId = pi.ProductId,
                                         SortOrder = pi.SortOrder
                                     }).FirstOrDefaultAsync();
            if(productImage == null)
            {
                return 0;
            }
            else
            {
                productImage.SortOrder = viewModel.SortOrder;
                productImage.Caption = viewModel.Caption;
                if(viewModel.ImageFile != null)
                {
                    productImage.ImagePath = await SaveFile(viewModel.ImageFile);
                    productImage.FileSize = viewModel.ImageFile.Length;
                }
            }
            await _context.SaveChangesAsync();
            await _storageService.SaveFileAsync(viewModel.ImageFile.OpenReadStream(), productImage.ImagePath);
            return productImage.Id;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";

            return fileName;
        }
    }
}
