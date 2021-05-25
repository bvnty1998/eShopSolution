using eShopSolution.AdminApp.Services;
using eShopSolution.AdminApp.Services.ManageFunction;
using eShopSolution.Utilities;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly IManageRoleClient _manageRole;
        private readonly IManageFunctionClient _manageFunction;
        private readonly IUserApiClient _userApiClient;
        public RoleController(IManageRoleClient manageRole, IManageFunctionClient manageFunction, IUserApiClient userApiClient)
        {
            _manageRole = manageRole;
            _manageFunction = manageFunction;
            _userApiClient = userApiClient;
        }
        #region Begin Role Index
        [HttpGet]
        public async Task<IActionResult> Index(RoleSearchViewModel viewModel)
        {
            // check roles
            var token = HttpContext.Session.GetString("Token");
            var id = new Guid(HttpContext.Session.GetString("UserId"));
            var roles = await _manageRole.GetRoleByUserId(token, id);
            var rolosAccess = roles.data.Any(x => x.RoleName == CommonController.VIEW);
            if (rolosAccess == false)
            {
                return Redirect("/Role/AccessDenied");
            };
            // end check roles


            var rs = await _manageRole.GetAllRole(token);
            ViewBag.Data = rs.data;
            ViewBag.statusMessage = TempData["Result"];
            return View();
        }
        #endregion End Role Index

        #region Begin Add Role
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
           

            var role = new AddRoleViewModel();
            ViewBag.statusMessage = TempData["ResultFaild"];
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(AddRoleViewModel viewModel)
        {
            // check roles
            var token = HttpContext.Session.GetString("Token");
            var id = new Guid(HttpContext.Session.GetString("UserId"));
            var roles = await _manageRole.GetRoleByUserId(token, id);
            var rolosAccess = roles.data.Any(x => x.RoleName == CommonController.ADD);
            if (rolosAccess == false)
            {
                return Redirect("/Role/AccessDenied");
            };
            // end check roles
           
            var rs = await _manageRole.AddRole(token, viewModel);
            if (rs.statusCode == 200)
            {
                TempData["Result"] = "Create Success";
                return Redirect("/Role/Index");
            }
            else
            {
                TempData["ResultFaild"] = "Create Faild";
                return Redirect("/Role/Index");
                
            }

        }

        #endregion End Add Role

        #region Access Denied
         public ActionResult AccessDenied()
        {
            return View();
        }
        #endregion Access Deied

        #region  Begin assign role for user
        [HttpGet]
        public async Task<IActionResult> AssginRoleForUser()
        {
            var token = HttpContext.Session.GetString("Token");
            var funcrions = await _manageFunction.GetAllFunction(token);
            var roles = await _manageRole.GetAllRole(token);
            var users = await _userApiClient.GetAllUser(token);
            ViewBag.UserId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(users.data, "Id", "UserName");
            ViewBag.ListFunction = funcrions.data;
            ViewBag.ListRole = roles.data;
            ViewBag.statusMessage = TempData["ResultAssginRoleForUser"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssginRoleForUser(List<PermissionViewModel> permission)
        {
            // check roles
            var token = HttpContext.Session.GetString("Token");
            var id = new Guid(HttpContext.Session.GetString("UserId"));
            var roles = await _manageRole.GetRoleByUserId(token, id);
            var rolosAccess = roles.data.Any(x => x.RoleName == CommonController.ASSGINROLE);
            if (rolosAccess == false)
            {
                return Json(new
                {
                    Code = HttpStatusCode.Forbidden,
                    Success = true,
                    Data = false,
                });
            };
            // end check roles
           
            var rs = await _manageRole.AssignRoleForUser(token, permission);
            if (rs.statusCode == 200)
            {
                TempData["ResultAssginRoleForUser"] = "Create Success";
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = rs,
                });
                
            }
            else
            {
                TempData["ResultAssginRoleForUser"] = "Create Faild";
                return Json(new
                {
                    Code = HttpStatusCode.BadRequest,
                    Success = false,
                    Data = rs,
                });
            }

        }
        #endregion End assign role for user

        #region Begin Get Role By Id
        public async Task<JsonResult> GetRoleById(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");
            var data =  await _manageRole.GetRoleByUserId(token, id);
            if(data.statusCode == 200)
            {
                return new JsonResult(new
                {
                    Status= HttpStatusCode.OK,
                    Success= true,
                    data = data.data
                });
            }
            else
            {
                return new JsonResult(new
                {
                    Status = HttpStatusCode.BadRequest,
                    Success = false,
                    data = data.data
                });
            }
        }
        #endregion End Get Role By Id
    }
}
