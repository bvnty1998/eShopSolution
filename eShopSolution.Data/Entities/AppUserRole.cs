using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
   public class AppUserRole : IdentityUserRole<Guid>
    {
        public int FunctionId { set; get; }
        public Function function { set; get; }
    }
}
