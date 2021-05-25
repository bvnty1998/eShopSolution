using eShopSolution.Application.System.Roles;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageRoleController : Controller
    {
        private IManageRoleService _manageRole;
        public ManageRoleController (IManageRoleService manageRole)
        {
            _manageRole = manageRole;
        }

        [HttpGet("GetRoleByUserId")]
        [Authorize]
        public async Task<IActionResult> GetRoleByUserId( Guid Id)
        {
            var result = await _manageRole.GetRoleByUser(Id);
            if(result != null)
            {
                return new OkObjectResult(new
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success",
                    data = result
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Get Role By Id Faild",
                    data = result
                });
            }
        }

        [HttpPost("AddRole")]
        [Authorize]
        public async Task<IActionResult> AddRole([FromBody]AddRoleViewModel viewModel)
        {
            var reslut =await _manageRole.AddRole(viewModel);
            if(reslut == true)
            {
                return new OkObjectResult(new
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success",

                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Success",

                });
            }
        }

        [HttpGet("GetAllRole")]
        [Authorize]
        public async Task<IActionResult> GetAllRole()
        {
            var result =await _manageRole.GetAllRole();
            if(result != null)
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.OK,
                    Message = "Success",
                    data = result
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    Message = "Get All Role Faild",
                    data = result
                });
            }
        }


        [HttpDelete("DeleteRoleForUserById")]
        [Authorize]
        public async Task<IActionResult> DeleteRoleForUserById(Guid Id)
        {
            var result = await _manageRole.DeleteRoleForUserById(Id);
            if (result != true)
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.OK,
                    Message = "Success",
                    data = result
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    Message = "Get All Role Faild",
                    data = result
                });
            }
        }

        [HttpPost("AssignRoleToUser")]
        [Authorize]
        public async Task<IActionResult> AssignRoleToUser([FromBody]List<PermissionViewModel> viewModel)
        {
            var result =await _manageRole.AssignRoleToUser(viewModel);
            if (result == true)
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.OK,
                    Message = "Success",
                    data = result
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    Message = "Faild",
                    data = result
                });
            }
        }
    }
}
