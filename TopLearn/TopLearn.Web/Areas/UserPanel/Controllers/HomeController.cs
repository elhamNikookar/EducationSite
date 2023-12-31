using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;
using TopLearn.Core.DTOs.UserViewModel;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {

        #region Constructor
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
                _userService = userService;
        }

        #endregion

        public IActionResult Index()
        {
            return View(_userService.GetUserInformation(User.Identity.Name));
        }

        #region Edit Profile

        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile()
        {
            return View(_userService.GetDataForEditProfileUser(User.Identity.Name));
        }

        [Route("UserPanel/EditProfile")]
        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel profile)
        {
            if (!ModelState.IsValid)
                return View(profile);

            _userService.EditProfile(User.Identity.Name, profile);
            ViewBag.IsSuccess = true;
            //Logout
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login?EditProfile=true"); 
        }

        #endregion

        #region  ChangePassword

        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("UserPanel/ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            string currentUserName = User.Identity.Name;

            if(!ModelState.IsValid)
                return View(changePassword);

            if (!_userService.CompareOldePassword(currentUserName, changePassword.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور فعلی صحیح نمی باشد.");
                return View(changePassword);
            }

            _userService.ChangeUserPassword(currentUserName, changePassword.Password);
            ViewBag.IsSuccess = true;
            return View();
        }

        #endregion


       

    }
}
