using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
  public  class AppUser: IdentityUser<Guid>
    {
        public string FristName { set; get; }
        public string LastName { set; get; }
        public DateTime BOD { set; get; }
    }
}
