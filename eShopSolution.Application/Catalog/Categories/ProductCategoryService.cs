using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities;
using eShopSolution.ViewModels.Catalog.Categories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly EShopDbContext _context;
        public ProductCategoryService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateProductCategory(CategoryCreateViewModel viewModel)
        {
            var category = new Category()
            {
               
                PrarentId = viewModel.PrarentId,
                SortOrder = viewModel.SortOrder,
                IsShowHome = viewModel.IsShowHome,
                Status = viewModel.Status,
                CategoryTranslations = new List<CategoryTranslation>()
                {
                    new CategoryTranslation()
                    {
                        Name = viewModel.Name,
                        SeoAlias = viewModel.SeoAlias,
                        LanguageId = viewModel.LanguageId,
                        SeoTitle = viewModel.SeoTitle,
                        SeoDescription = viewModel.SeoDescription
                    }
                }
            };
            var rs =_context.Categories.Add(category);
          
            await _context.SaveChangesAsync();
            return category.Id;

        }

        public async Task<bool> DeleteProductCategory(int CategoryId)
        {
            var category = await (_context.Categories.Where(x => x.Id == CategoryId)).FirstOrDefaultAsync();
            if (category == null) throw new eShopException($"Can not find Category{CategoryId}");
            _context.Categories.Remove(category);
             await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CategorySearchViewModel> GetCategoryById(int Id)
        {
            var category = await (from c in _context.Categories
                                  join ct in _context.CategoryTranslations
                                  on c.Id equals ct.CategoryId
                                  where c.Id == Id
                                  select new CategorySearchViewModel() { 
                                  Id = c.Id,
                                  Name =ct.Name, 
                                  IsShowHome = c.IsShowHome,
                                  PrarentId = c.PrarentId,
                                  SortOrder = c.SortOrder,
                                  Status = c.Status,
                                  LanguageId = ct.LanguageId,
                                  SeoAlias = ct.SeoAlias,
                                  SeoDescription = ct.SeoDescription,
                                  SeoTitle = ct.SeoTitle
                                  }).FirstOrDefaultAsync();
            if (category == null) throw new eShopException($"Can not find Category{Id}");
            return category;
        }

        public async Task<int> UpdateProductCategory(CategoryUpdateViewModel viewModel)
        {
            var category = await(_context.Categories.Where(x => x.Id == viewModel.Id)).FirstOrDefaultAsync();
            if (category == null) throw new eShopException($"Can not find Category{viewModel.Id}");
            category.PrarentId = viewModel.PrarentId;
            category.SortOrder = viewModel.SortOrder;
            category.Status = viewModel.Status;
            category.IsShowHome = viewModel.IsShowHome;
            var categoryTransition = await (_context.CategoryTranslations.Where(x => x.CategoryId == viewModel.Id)).FirstOrDefaultAsync();
            if (categoryTransition == null) throw new eShopException($"Can not find Category{viewModel.Id}");
            categoryTransition.Name = viewModel.Name;
            categoryTransition.SeoAlias = viewModel.SeoAlias;
            categoryTransition.SeoTitle = viewModel.SeoTitle;
            
            _context.Entry(category).State = EntityState.Modified;
            _context.Entry(categoryTransition).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}
