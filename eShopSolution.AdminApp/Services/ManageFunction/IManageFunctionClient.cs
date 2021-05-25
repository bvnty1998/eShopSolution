using eShopSolution.ViewModels.System.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services.ManageFunction
{
  public  interface IManageFunctionClient
    {
        Task<ResultApiGetAllFunction> GetAllFunction(string token);
        Task<ResultApiAddFunction> AddFunction(string token, AddFunctionViewModel viewModel);
    }
}
