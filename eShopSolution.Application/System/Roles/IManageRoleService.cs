using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Roles
{
   
    public interface IManageRoleService
    {
        Task<List<GetPermissionViewModel>> GetRoleByUser(Guid id);
        Task<bool> AddRole(AddRoleViewModel viewModel);
        Task<List<RoleViewModel>> GetAllRole();
        Task<bool> AssignRoleToUser(List<PermissionViewModel> viewModel);

        Task<bool> DeleteRoleForUserById(Guid Id);
    }
}
