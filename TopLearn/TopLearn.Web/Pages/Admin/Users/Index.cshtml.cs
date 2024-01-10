using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs.UserViewModel;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{

    //[PermissionChecker(2)]
    public class IndexModel : PageModel
    {

        #region Constructor

        private readonly IUserService _userService;
        public UsersForAdminViewModel UserForAdminViewModel { get; set; }

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        #endregion


        public void OnGet(int pageId=1,string filterUserName="",string filterEmail="")
        {
            UserForAdminViewModel = _userService.GetUsers(pageId,filterEmail,filterUserName);
        }


    }
}