﻿using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Roles

        List<Role> GetRoles();
        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void AddRolesToUser(List<int> roleIds, int userId);
        void EditRolesUser(int userId, List<int> rolesId);
        void UpdateRole(Role role);
        void DeleteRole(Role role);

        #endregion


        #region Permission

        List<Permission> GetAllPermission();
        void AddPermissionToRole(int roleId, List<int> permissions);
        List<int> PermissionRole(int roleId);
        void UpdatePermissionsRole(int roleId, List<int> permissions);
        bool CheckPermission(int permissionId, string userName);

        #endregion
    }
}
