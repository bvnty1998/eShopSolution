using eShopSolution.ViewModels.System.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Functions
{
   public interface IFunctionService
    {
        Task<bool> AddFunction(AddFunctionViewModel viewModel);
        Task<List<FunctionViewModel>> GetAllFunction();
    }
}
