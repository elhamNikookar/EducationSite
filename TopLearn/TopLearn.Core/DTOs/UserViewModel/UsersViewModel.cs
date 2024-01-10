using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.DTOs.UserViewModel
{
    public class UsersForAdminViewModel
    {
        public List<User> Users { get; set; }
        public int CurrentPage { set; get; }
        public int PageCount { set; get; }
    }

    public class CreateUserViewModel
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

        public IFormFile? UserAvatar { get; set; }

        public List<int>? SelectedRoles { get; set; } = new List<int>();
    }


    public class EditUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد.")]
        public string? Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        public string? Password { get; set; }

        public List<int>? UserRoles { get; set; } = new List<int>();

        public string? AvatarName { get; set; }

        public IFormFile? UserAvatar { get; set; }

    }

}
