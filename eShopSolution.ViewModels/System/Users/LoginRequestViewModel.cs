using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
   public class LoginRequestViewModel
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }
        public bool RemenberMe { set; get; }

    }
}
