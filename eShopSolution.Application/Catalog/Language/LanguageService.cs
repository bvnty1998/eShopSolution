using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities;
using eShopSolution.ViewModels.Catalog.Language;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Language
{
    public class LanguageService : ILanguageService
    {
        private readonly EShopDbContext _context;
        public LanguageService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateLanguage(LanguageCreateViewModel viewModel)
        {
            var language = new Data.Entities.Language() {
             Id = viewModel.Id,
             Name = viewModel.Name,
             IsDefault = viewModel.IsDefault
            };
            await _context.Languages.AddAsync(language);
            await _context.SaveChangesAsync();
            return language.Id;
        }

        public async Task<bool> Delete(string id)
        {
            var langue = await (_context.Languages.Where(x => x.Id == id)).FirstOrDefaultAsync();
            if (langue == null) throw new eShopException($"Can not find language {id}");
            _context.Languages.Remove(langue);
            return true;
        }

        public async Task<LanguageSearchViewModel> GetLanguageById(string id)
        {
            var langue = await(from l in _context.Languages
                               where l.Id == id
                               select new LanguageSearchViewModel() { 
                               Id = l.Id,
                               Name = l.Name,
                               IsDefault = l.IsDefault
                               }).FirstOrDefaultAsync();
            if (langue == null) throw new eShopException($"Can not find language {id}");
            return langue;
        }

        public async Task<string> UpdateLanguage(LanguageUpdateViewModel viewModel)
        {
            var langue = await(_context.Languages.Where(x => x.Id == viewModel.Id)).FirstOrDefaultAsync();
            if (langue == null) throw new eShopException($"Can not find language {viewModel.Id}");
            viewModel.Name = viewModel.Name;
            viewModel.IsDefault = viewModel.IsDefault;
           await _context.SaveChangesAsync();
            return viewModel.Id;
        }
    }
}
