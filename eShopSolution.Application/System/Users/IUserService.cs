using eShopSolution.ViewModels.Common;
using eShopSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Users
{
  public  interface IUserService
    {
        Task<string> Authencate(LoginRequestViewModel request);
        Task<UserViewModel> RegisterRequest(RegisterRequestViewModel request);
        Task<UserViewModel> GetUserById(Guid id);
        Task<PageResult<UserViewModel>> GetUserPaging(UserPagingRequest request);
        Task<int> DeleteUserById(Guid userId);
        Task<UserViewModel> UpdateUser(UpdateUserViewModel request);
        Task<List<UserViewModel>> GetAllUser();
    }
}
