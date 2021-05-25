using DevExpress.Xpo;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
    public class ManageRoleService : IManageRoleService
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private readonly EShopDbContext _context;
        public ManageRoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,EShopDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<bool> AddRole(AddRoleViewModel viewModel)
        {
            var role = new AppRole()
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };
           var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<RoleViewModel>> GetAllRole()
        {
           var listRole =  _roleManager.Roles.ToList();
            var Roles = new List<RoleViewModel>();
            foreach(var item in listRole)
            {
                var role = new RoleViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description
                };
                Roles.Add(role);
            }
            return Roles;
        }

        public async Task<List<GetPermissionViewModel>> GetRoleByUser(Guid id)
        {
            var user =  ( from p in _context.Permissions
                              where p.UserId == id
                              select new GetPermissionViewModel()
                              {
                                  FunctionId = p.FunctionId,
                                  FunctionName = p.FunctionName,
                                  UserId = p.UserId,
                                  RoleId = p.RoleId,
                                  RoleName = p.RoleName
                                  
                              }).ToList();
            
            return user;
            //throw new NotImplementedException();

        }

        public async Task<bool> AssignRoleToUser(List<PermissionViewModel> viewModel)
        {
           foreach(var data in viewModel)
            {
                foreach(var item in data.roles)
                {
                    var permission = new Permission()
                    {
                        UserId = data.userId,
                        RoleId = item.Id,
                        RoleName = item.Name,
                        FunctionId = data.functionId,
                        FunctionName = data.functionName
                    };
                    await _context.Permissions.AddAsync(permission);
                }
            }
           await _context.SaveChangesAsync();
            return true;
           
        }

        public async Task<bool> DeleteRoleForUserById(Guid Id)
        {
            var roleforuser= await _context.Permissions.Where(x => x.UserId == Id).ToListAsync();
            _context.Permissions.RemoveRange(roleforuser);
            return true;
        }
    }
}
