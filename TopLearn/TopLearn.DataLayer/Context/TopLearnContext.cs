using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Course;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.DataLayer.Context
{
    public class TopLearnContext : DbContext
    {

        #region Constructor

        public TopLearnContext(DbContextOptions<TopLearnContext> options) : base(options) 
        {
                
        }

        #endregion

        #region User DataBase

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion

        #region Wallet DataBase 

        public DbSet<WalletType> WalletTypes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        #endregion


        #region Permission DataBase

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

        #region Course DataBase

        public DbSet<CourseGroup> CourseGroups { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Role>()
               .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<CourseGroup>()
               .HasQueryFilter(g => !g.IsDelete);

            base.OnModelCreating(modelBuilder);
        }
    }
}
