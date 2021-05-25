using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Roles
{
   public class PermissionViewModel
    {
        public Guid userId { set; get; }
        public List<RoleViewModel> roles { set; get; }
        public int functionId { set; get; }
        public string functionName { set; get; }
    }
}
