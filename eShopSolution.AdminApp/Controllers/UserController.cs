
using eShopSolution.AdminApp.Services;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuaration;
        public UserController (IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuaration = configuration;
        }
        #region index   
        [HttpGet]
        public async Task<IActionResult> Index(UserPagingRequest request)
        {
            request.PageIndex = request.PageIndex == 0?1:request.PageIndex;
            request.PageSize = 1;
            request.keyword = "";
            var token = HttpContext.Session.GetString("Token");
            var users = await _userApiClient.UserPagingRequest(request, token);
            var statusMessage = TempData["Result"];
            if(statusMessage != null)
            {
                ViewBag.statusMessage = statusMessage;
            }
            ViewBag.Data = users.Items;
            ViewBag.TotalRecord = users.TotalRecord;
            return View(request);
        }
        #endregion
      
        #region Create User
        [HttpGet]
        public IActionResult CreateUser()
        {
            var user = new RegisterRequestViewModel();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterRequestViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var token = HttpContext.Session.GetString("Token");
                var reponse = await _userApiClient.CreateUser(viewModel, token);
                if (reponse != null)
                {
                    TempData["Result"] = "Create Success";
                    return Redirect("Index");
                }
                else
                {
                    ViewBag.Erorr = "Create user failed";
                    return View();
                }
            }
            else
            {
                ViewBag.Erorr = "Create user failed";
                return View();
            }

        }
        #endregion Create User

        #region Begin Login Logout
        // Login
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // xoá các seesion (logout) trước khi login 
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var token = await _userApiClient.Authenticate(viewModel);
               
                if( token != "BadRequest")
                {
                    // Giải mã token 
                    var userprincipal = ValidateToken(token);
                    var authProperties = new AuthenticationProperties()
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                        IsPersistent = false
                    };
                   await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        userprincipal,
                        authProperties
                        );
                    string id = userprincipal.Claims.FirstOrDefault(x => x.Type == "Id").Value.ToString();
                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("UserId", id);
                    return Redirect("/Home/Index");
                }
                else
                {
                    return Redirect("Index");
                }
            }
            return Redirect("Index");

        }

       
        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/User/Login");
        }
        #endregion End Login Logout

        #region Begin Delete User
        public async Task<IActionResult> DeleteUserById(Guid Id)
        {
            var token = HttpContext.Session.GetString("Token");
            var rs =await _userApiClient.DeleteUserById(Id, token);
            if(rs == 200)
            {
                return Json(new
                {
                    status = "Success",
                    statusCode = rs
                });
            }
            else
            {
                return Json(new
                {
                    status = "Error",
                    statusCode = rs
                });
            }
        }
        #endregion End Delete User

        #region Begin Edit USer
        public async Task<IActionResult> EditUser(Guid id)
        {
            var token = HttpContext.Session.GetString("Token");
            var reponse = await _userApiClient.GetUserById(id,token);
            var user = new UpdateUserViewModel();
            user.FirstName = reponse.FirstName;
            user.LastName = reponse.LastName;
            user.PhoneNumber = reponse.PhoneNumber;
            user.Id = reponse.Id;
            user.Dob = reponse.DOB;
            user.Email = reponse.Email;
            var statusMessage = TempData["ResultFaild"];
            if (statusMessage != null)
            {
                ViewBag.statusMessage = statusMessage;
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUserViewModel viewModel)
        {
            var token = HttpContext.Session.GetString("Token");
            var response = await _userApiClient.UpdateUser(viewModel, token);
            if(response.UserName != null)
            {
                TempData["Result"] = "Update Success";
                return Redirect("/User/Index");
            }
            else
            {
                TempData["ResultFaild"] = "Update faild ";
                return Redirect("EditUser");
            }
        }
        #endregion END Edit User

        #region BĐHàm giải mã JWT
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = "abc";
            validationParameters.ValidIssuer ="abc";
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuaration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
        #endregion KT Hàm giải mã JWT


        //[HttpGet]
        //public async Task<IActionResult> UserPaging()
        //{
            
        //}
    }
}
