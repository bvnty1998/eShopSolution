using eShopSolution.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
    public interface IManageRoleClient
    {
        Task<ResultApiAddRole> AddRole(string token,AddRoleViewModel viewModel);
        Task<ResultApiGetRoleAll> GetAllRole(string token);
        Task<ResultApiAssignRoleForUser> AssignRoleForUser(string token, List<PermissionViewModel> viewModel);
        Task<ResultApiGettRoleById> GetRoleByUserId(string token, Guid Id);
    }
}
