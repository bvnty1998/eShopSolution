using eShopSolution.Application.System.Functions;
using eShopSolution.ViewModels.System.Functions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    public class FunctionController : Controller
    {
        public IFunctionService _functionService;
        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }
        [HttpGet("GetAllFunction")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var rs = await _functionService.GetAllFunction();
            if(rs != null)
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.OK,
                    Message = "Success",
                    data = rs
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    Message = "Get All Function Faild",
                    data = rs
                });
            }
            
        }

        [HttpPost("AddFunction")]
        [Authorize]
        public async Task<IActionResult> AddFunction([FromBody]AddFunctionViewModel viewModel)
        {
            var rs =await _functionService.AddFunction(viewModel);
            if(rs == true)
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.OK,
                    Message = "Success",
                    data = rs
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    Message = "Create Function Faild",
                    data = rs
                });
            }
        }
    }
}
