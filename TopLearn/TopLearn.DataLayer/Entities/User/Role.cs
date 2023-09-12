using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Permissions;

namespace TopLearn.DataLayer.Entities.User
{
    public class Role
    {
        public Role()
        {
            
        }

        [Key]
        public int RoleId { get; set; }

        [Display(Name ="عنوان نقش")]
        [Required(ErrorMessage ="لطفا{0} را وارد کنید.")]
        [MaxLength(200,ErrorMessage ="{0} نمی تواند بیش از{1} کاراکتر  اشته باشد.")]
        public string RoleTitle { get; set; }

        public bool IsDelete { get; set; }

        #region Relations

        public virtual List<UserRole> UserRoles { get; set; }
        public List<RolePermission> RolePermissions { get; set; }


        #endregion
    }
}
