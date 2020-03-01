using Forum.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Data
{
    public class DataSeeder
    {
        #region

        private ApplicationDbContext _dbContext;
        #endregion

        #region "Constructor"

        public DataSeeder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region "Public Methods"

        /// <summary>
        /// Seeds the Super user and Admin role.
        /// </summary>
        /// <returns></returns>
        public Task SeedSuperUser()
        {
            var roleStore = new RoleStore<IdentityRole>(_dbContext);
            var userStore = new UserStore<ApplicationUser>(_dbContext);

            var user = new ApplicationUser
            {
                UserName = "ForumAdmin",
                NormalizedUserName = "forumadmin",
                Email = "admin@dev.com",
                NormalizedEmail = "admin@dev.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var hashed = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = hashed.HashPassword(user, "admin");

            var hasAdminRole = _dbContext.Roles
                .Any(roles => roles.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase));

            if (!hasAdminRole)
            {
                roleStore.CreateAsync(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "admin"
                });
            }

            var hasSuperUser = _dbContext.Users
                .Any(u => u.NormalizedUserName.Equals(user.UserName, StringComparison.OrdinalIgnoreCase));

            if (!hasSuperUser)
            {
                userStore.CreateAsync(user);
                userStore.AddToRoleAsync(user, "admin");
            }

            _dbContext.SaveChanges();

            return Task.CompletedTask;
        }
        #endregion
    }
}
