﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{

    //[PermissionChecker(9)]
    public class DeleteRoleModel : PageModel
    {
        #region Constructor

        [BindProperty]
        public Role Role { get; set; }

        private readonly IPermissionService _permissionService;

        public DeleteRoleModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        #endregion
        public void OnGet(int id)
        {
            Role = _permissionService.GetRoleById(id);
        }

        public IActionResult OnPost()
        {
            _permissionService.DeleteRole(Role);

            return RedirectToPage("Index");
        }
    }
}