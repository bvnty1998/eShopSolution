using eShopSolution.ViewModels.Catalog.Language;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Language
{
   public interface ILanguageService
    {
        Task<string> CreateLanguage(LanguageCreateViewModel viewModel);
        Task<string> UpdateLanguage(LanguageUpdateViewModel viewModel);
        Task<bool> Delete(string id);
        Task<LanguageSearchViewModel> GetLanguageById(string id);
    }
}
