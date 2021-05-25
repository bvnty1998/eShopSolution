using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Roles
{
   public class GetPermissionViewModel
    {
        public Guid UserId { set; get; }
        public int FunctionId { set; get; }
        public Guid RoleId { set; get; }
        public string RoleName { set; get; }
        public string FunctionName { set; get; }
    }
}
