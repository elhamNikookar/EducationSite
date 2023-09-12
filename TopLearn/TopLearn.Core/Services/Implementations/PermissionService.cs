using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services
{
    public class PermissionService : IPermissionService
    {

        #region Constructor

        private readonly TopLearnContext _context;

        public PermissionService(TopLearnContext context)
        {
            _context = context;
        }

        #endregion


        #region Roles

        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public void AddRolesToUser(List<int> roleIds, int userId)
        {
            foreach (int roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }

            _context.SaveChanges();
        }

        public void EditRolesUser(int userId, List<int> rolesId)
        {
            //Delete All Roles User
            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));

            //Add New Roles
            AddRolesToUser(rolesId, userId);
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            role.IsDelete = true;
            UpdateRole(role);
        }


        #endregion


        #region Permission

        public List<Permission> GetAllPermission()
        {
            return _context.Permissions.ToList();
        }

        public void AddPermissionToRole(int roleId, List<int> permissions)
        {
            foreach (var p in permissions)
            {
                _context.RolePermissions.Add(new RolePermission()
                {
                    PermissionId = p,
                    RoleId = roleId
                });
            }

            _context.SaveChanges();
        }

        public List<int> PermissionRole(int roleId)
        {
            return _context.RolePermissions
                .Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionId)
                .ToList();
        }

        public void UpdatePermissionsRole(int roleId, List<int> permissions)
        {
            _context.RolePermissions
                .Where(p => p.RoleId == roleId).ToList()
                .ForEach(p => _context.RolePermissions.Remove(p));

            AddPermissionToRole(roleId, permissions);
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int userId = _context.Users.Single(u => u.UserName == userName).UserId;

            List<int> userRoles = _context.UserRoles
                .Where(r => r.UserId == userId)
                .Select(r => r.RoleId)
                .ToList();

            if (!userRoles.Any())
                return false;

            List<int> rolesPermission = _context.RolePermissions
                .Where(p => p.PermissionId == permissionId)
                .Select(p => p.RoleId)
                .ToList();

            
            return rolesPermission.Where(p => userRoles.Contains(p)).Any();
        }

        #endregion

    }
}
