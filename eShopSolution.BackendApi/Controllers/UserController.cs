using eShopSolution.Application.System.Users;
using eShopSolution.Data.EF;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> authenticate([FromBody] LoginRequestViewModel request)
        {
            var resultToken = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("Username or password incorrect");
            }
            else
            {
                return Ok(resultToken);
            }
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> register([FromBody] RegisterRequestViewModel request)
        {
            var result = await _userService.RegisterRequest(request);
            if (result.UserName == null)
            {
                return BadRequest("Register is failed");
            }
            else
            {
                return new OkObjectResult(new {
                    statusCode = HttpStatusCode.OK,
                    data = result
                });
            }
        }


        [HttpPost("GetUserPaging")]
        [Authorize]
        public async Task<IActionResult> GetUserPaging([FromBody] UserPagingRequest request)
        {
            var result = await _userService.GetUserPaging(request);
            if (request == null)
            {
                return BadRequest();
            }
            else
            {
                return new OkObjectResult(new {
                    StatusCode = HttpStatusCode.OK,
                    Data = result
                });
            }
        }
        [HttpGet("GetAllUser")]
        [Authorize]
        public async Task<IActionResult> GetAllUser()
        {

            var data = await _userService.GetAllUser();
            if(data != null)
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.OK,
                    message = "Success",
                    data = data
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.BadRequest,
                    message = "Faild",
                    data = data
                });
            }
        }


        [HttpGet("GetUserById")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid Id)
        {
            var result = await _userService.GetUserById(Id);
            if (result.UserName != null )
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.OK,
                    data = result
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.NotFound,
                    data = result
                });
            }
        }

        [HttpDelete("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            var rs =await _userService.DeleteUserById(id);
            return Ok(rs);
        }

        [HttpPost("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserViewModel viewModel)
        {
            var response =await _userService.UpdateUser(viewModel); 
            if(response.UserName != null)
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.OK,
                    data = response
                });
            }
            else
            {
                return new OkObjectResult(new
                {
                    statusCode = HttpStatusCode.NotFound,
                    data = response
                });
            }
        }

    }
}
