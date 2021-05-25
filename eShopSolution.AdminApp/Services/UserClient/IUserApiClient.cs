using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShopSolution.AdminApp.Services
{
  public  interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequestViewModel viewModel);
        Task<PageResult<UserViewModel>> UserPagingRequest(UserPagingRequest viewModel, string token);
        Task<UserViewModel> CreateUser(RegisterRequestViewModel viewModel, string token);
        Task<int> DeleteUserById(Guid id, string token);
        Task<UserViewModel> GetUserById(Guid Id, string token);
        Task<UserViewModel> UpdateUser(UpdateUserViewModel viewModel, string token);
        Task<ResultApiGetAllUser> GetAllUser(string token);
    }
}
