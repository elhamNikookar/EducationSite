using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs.UserViewModel;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{

    [PermissionChecker(4)]
    public class EditUserModel : PageModel
    {

        #region Constructor

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }

        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;


        public EditUserModel(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        #endregion

        public void OnGet(int id)
        {
            EditUserViewModel = _userService.GetUserForShowInEditMode(id);
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
                return Page();

            _userService.EditUserFromAdmin(EditUserViewModel);

            //Edit Roles
            _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);

            return RedirectToPage("Index");
        }
    }
}
