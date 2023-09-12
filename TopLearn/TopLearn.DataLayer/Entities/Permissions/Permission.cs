using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopLearn.DataLayer.Entities.Permissions
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [Display(Name = "عنوان نقش")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        [Required(ErrorMessage = "لطفا{0} را وارد کنید.")]
        public string PermissionTitle { get; set; }

        public int? ParentId { get; set; }


        [ForeignKey("ParentId")]
        public List<Permission> Permissions { get; set; }

        public List<RolePermission> RolePermissions { get; set; }
    }
}
