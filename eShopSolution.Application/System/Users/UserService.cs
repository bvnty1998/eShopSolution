using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities;
using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly EShopDbContext _context; 


        public UserService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager
            ,RoleManager<AppRole> roleManager, IConfiguration config, EShopDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;
        }
        #region Begin authencate user
        public async Task<string> Authencate(LoginRequestViewModel request)
        {
            var user =await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null;
            var result = await _signInManager.PasswordSignInAsync(user, request.Password,request.RemenberMe,true);
            if(!result.Succeeded )
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
               new Claim("Email", user.Email),
               new Claim("FristName" ,user.FristName),
               new Claim("Id",user.Id.ToString()),
               new Claim("Roles",string.Join(";",roles))
           };
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_config["AppSettings:Secret"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("abc",
                "abc",
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
            //throw new NotImplementedException();
        }
        #endregion End authencate user

        #region Begin Register account
        public async Task<UserViewModel> RegisterRequest(RegisterRequestViewModel request)
        {
            var user = new AppUser()
            {
                BOD = request.Dob,
                Email = request.Email,
                FristName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName
                
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if(result.Succeeded)
            {
                var rs =  await this.GetUserById(user.Id);
                return rs;
            }
            else
            {
                var rs = new UserViewModel();
                return rs;
            }
        }
        #endregion End Register account

        #region Begin Get User
        // Get user paging
        public async Task<PageResult<UserViewModel>> GetUserPaging(UserPagingRequest request)
        {
            // b1 create query
            var query =( from u in  _userManager.Users
                         select new UserViewModel() { 
                          Id = u.Id,
                          Email = u.Email,
                          FirstName = u.FristName,
                          LastName = u.LastName,
                          DOB = u.BOD,
                          PhoneNumber = u.PhoneNumber,
                          UserName= u.UserName
                         });
            // b2 select user by keyword
            if(! string.IsNullOrEmpty(request.keyword))
            {
                query = query.Where(x => x.LastName.Contains(request.keyword) || x.UserName.Contains(request.keyword));
            }
            // b3 count record
            var totalRecord = await query.CountAsync();
            // b4 paging
           var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
            PageResult<UserViewModel> pageResult = new PageResult<UserViewModel>()
            {
                TotalRecord = totalRecord,
                Items = data
            };
            return pageResult;
        }

        // Get User By Id
        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var result = await _userManager.FindByIdAsync(id.ToString());
            if(result != null )
            {
                var user = new UserViewModel()
                {
                    Id = result.Id,
                    Email = result.Email,
                    FirstName = result.FristName,
                    LastName = result.LastName,
                    PhoneNumber = result.PhoneNumber,
                    UserName = result.UserName,
                    DOB = result.BOD
                };
                return user;
            }
           else
            {
                var user = new UserViewModel();
                return user;
            }
        }

        // Get All User
        public async Task<List<UserViewModel>> GetAllUser()
        {
            var data = await _userManager.Users.ToListAsync();
            var users = new List<UserViewModel>();
            foreach (var item in data)
            {
                var user = new UserViewModel()
                {
                    Id = item.Id,
                    DOB = item.BOD,
                    Email = item.Email,
                    FirstName = item.FristName,
                    LastName = item.LastName,
                    PhoneNumber = item.PhoneNumber,
                    UserName = item.UserName
                };
                users.Add(user);
            }
            return users;
        }

        #endregion End Get User

        #region Delete User
        public async Task<int> DeleteUserById(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user== null)
            {
                return 404;
            }
            var reponse = await _userManager.DeleteAsync(user);
            if(reponse.Succeeded)
            {
                return 200;
            }
            else
            {
                return 400;
            }
        }
        #endregion End Delete user

        #region Begin Update User
        public async Task<UserViewModel> UpdateUser(UpdateUserViewModel request)
        {
            var result = await _userManager.FindByIdAsync(request.Id.ToString());
            if(result != null)
            {
                result.FristName = request.FirstName;
                result.LastName = request.LastName;
                result.Email = request.Email;
                result.PhoneNumber = request.PhoneNumber;
                if(! string.IsNullOrEmpty(request.Password))
                {
                    result.PasswordHash= _userManager.PasswordHasher.HashPassword(result,request.Password);
                }
               var rs=await _userManager.UpdateAsync(result);
                if(rs.Succeeded)
                {
                    var user =await this.GetUserById(result.Id);
                    return user;
                }
                else
                {
                    var user = new UserViewModel();
                    return user;
                }
            }
            else
            {
                var user = new UserViewModel();
                return user;
            }
        }
        #endregion End Update User

    }
}