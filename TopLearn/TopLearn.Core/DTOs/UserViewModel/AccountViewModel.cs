using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.Core.DTOs.UserViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        public string UserName { get; set; }


        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }


        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        public string Password { get; set; }

        [Display(Name = "تکرار کلمه عبور")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        [Compare("Password", ErrorMessage = "کلمه های عبور مغابرت دارند.")]
        public string RePassword { get; set; }
    }

    public class LoginViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }


        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        public string Password { get; set; }

        [Display(Name = "مرا بخاطر بسپار!")]
        public bool RememberMe { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string ActiveCode { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        public string Password { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        [Compare("Password", ErrorMessage = "کلمه های عبور مغابرت دارند.")]
        public string RePassword { get; set; }
    }


}
