using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs.UserViewModel;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {


        #region Constructor

        private readonly IUserService _userService;

        public WalletController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion



        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ViewBag.ListWallet = _userService.GetWalletUser(User.Identity.Name);
            return View();
        }

        [Route("UserPanel/Wallet")]
        [HttpPatch]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.ListWallet = _userService.GetWalletUser(User.Identity.Name);
                return View(charge);
            }

            int walletId = _userService.ChargeWallet(User.Identity.Name, charge.Amount, "شارژ حساب");

            //ToDo online Payment
            #region Online Payment

            var payment = new ZarinpalSandbox.Payment(charge.Amount);

            var res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:7006/OnlinePayment/" + walletId);

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/startPay/" + res.Result.Authority);
            }

            #endregion

            return null;
            
        }
   
    }
}
