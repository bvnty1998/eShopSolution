using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
   public class RegisterRequestViewModel
    {

        [Display(Name = "Tên")]
        [Required(ErrorMessage ="Yêu cầu nhập tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Yêu cầu nhập Họ")]
        public string LastName { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Yêu cầu nhập ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Display(Name = "Hòm thư")]
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Yêu cầu nhập sđt")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Yêu cầu nhập tên đăng nhâp")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yêu cầu nhập xác minh mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}
