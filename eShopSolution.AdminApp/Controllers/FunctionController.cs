using eShopSolution.AdminApp.Services.ManageFunction;
using eShopSolution.ViewModels.System.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class FunctionController : Controller
    {
        private readonly IManageFunctionClient _functionClient;
        public FunctionController (IManageFunctionClient functionClient)
        {
            _functionClient = functionClient;
        }
        #region  begin Function index
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("Token");
            var rs = await _functionClient.GetAllFunction(token);
            ViewBag.Data = rs.data;
            ViewBag.statusMessage = TempData["Result"];
            var function = new AddFunctionViewModel();
            return View(function);
        }
        #endregion  end function index

        #region begin add function
        public async Task<IActionResult> CreateFunction(AddFunctionViewModel viewModel)
        {
            var token = HttpContext.Session.GetString("Token");
            var rs = await _functionClient.AddFunction(token, viewModel);
            if(rs.statusCode == 200)
            {
                TempData["Result"] = "Create Success";
                return Redirect("/Function/Index");
            }
            else
            {
                TempData["Result"] = "Create Faild";
                return Redirect("/Function/Index");
            }
        }
        #endregion end add function
    }
}
