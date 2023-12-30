using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TopLearn.Core.Convertors;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;
using TopLearn.Core.Senders;
using TopLearn.Core.DTOs.UserViewModel;

namespace TopLearn.Web.Controllers
{
    public class AccountController : Controller
    {

        #region Constructor

        private readonly IUserService _userService;
        private readonly IViewRenderService _viewRenderService;
        public AccountController(IUserService userService, IViewRenderService viewRenderService)
        {
            _userService = userService;
            _viewRenderService = viewRenderService;
        }

        #endregion


        #region Register

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userService.IsExistEmail(register.Email.FixEmail()))
            {
                ModelState.AddModelError("Email", "ایمیل قبلا ثبت شده است.");
                return View(register);
            }

            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "این نام کاربری از قبل رزرو شده است.");
                return View(register);
            }

            //Register
            User user = new User()
            {
                ActiveCode = NameGenerator.GeneratorUniqCode(),
                Email = register.Email.FixEmail(),
                IsActive = false,
                Password = register.Password.EncodePasswordMd5(),
                RegisterDate = DateTime.Now,
                UserAvatar = "Default.jpg",
                UserName = register.UserName
            };

            _userService.AddUser(user);

            // send Activation email
            //string body = _viewRenderService.RenderToStringAsync("_ActiveEmail", user);
            //SendEmail.Send(user.Email, "ایمیل فعالسازی", body);


            return View("SuccessRegister", user);
        }

        #endregion


        #region Login

        [Route("Login")]
        public IActionResult Login(bool EditProfile = false)
        {
            ViewBag.EditProfile = EditProfile;
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.LoginUser(login);

            if (user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    };

                    HttpContext.SignInAsync(principal, properties);

                    ViewBag.IsSuccess = true;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری فعال نمی باشد.");
                }
            }
            else

                ModelState.AddModelError("Email", "حساب کاربری یافت نشد!");


            return View(login);
        }


        #endregion


        #region Active Account

        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            return View();
        }
        #endregion


        #region Logout

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion


        #region ForgotPassword

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            string fixedEmail = forgot.Email.FixEmail();
            DataLayer.Entities.User.User user = _userService.GetUserByEmail(fixedEmail);

            if (user == null)
            {
                ModelState.AddModelError("Email", "حساب کاربری یافت نشد.");
                return View();
            }

            string bodyEmail = _viewRenderService.RenderToStringAsync("_ForgotPassword", user);
            ViewBag.IsSuccess = true;

            return View();
        }

        #endregion


        #region Reset Password

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }


        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);

            DataLayer.Entities.User.User user = _userService.GetUserByActiveCode(resetPassword.ActiveCode);

            if (user == null)
                return NotFound();

            string hashNewPassword = resetPassword.Password.EncodePasswordMd5();
            user.Password = hashNewPassword;
            _userService.UpdateUser(user);

            return Redirect("/Login");


        }

        #endregion

    }

}
