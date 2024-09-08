using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myshop.Entities.Models;
using myshop.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.DbInitializer
{
    public class Initializer : IDbInitializer
    {
         
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public Initializer(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext
            )
        {
            _userManager = userManager; 
            _roleManager = roleManager;
            _applicationDbContext= applicationDbContext;
        }

        public void initializer()
        {
            try
            {
                if (_applicationDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _applicationDbContext.Database.Migrate();
                }
            }
            catch (Exception)
            { 
                throw;
            }


            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "Admin",
                    Name="Admin",
                    PhoneNumber="01287517894",
                    Email="Admin@gmail.com",
                    Address="cairo",
                    City="cairo"
                },"Adminnn@12").GetAwaiter().GetResult();

                ApplicationUser user = _applicationDbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "Admin@gmail.com");
                _userManager.AddToRoleAsync(user,SD.AdminRole).GetAwaiter().GetResult();

            }
            return;
        }
    }
}
