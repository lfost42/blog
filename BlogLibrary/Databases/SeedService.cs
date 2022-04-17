using BlogLibrary.Data;
using BlogLibrary.Models;
using BlogLibrary.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Databases
{
    public class SeedService
    {
        //DEMO PURPOSES ONLY
        
        private readonly BlogContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModel> _userManager;
        public SeedService(BlogContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<UserModel> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsnc()
        {
            await _dbContext.Database.MigrateAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (_dbContext.Roles.Any()) return;
            foreach (var role in Enum.GetNames(typeof(Role)))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private async Task SeedUsersAsync()
        {
            if (_dbContext.Users.Any()) return;
            
            var ownerUser = new UserModel()
            {
                Email = "owner@myblog.com",
                UserName = "owner@myblog.com",
                FirstName = "Owner",
                LastName = "Demobarista",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(ownerUser, "Abc1234!");
            await _userManager.AddToRoleAsync(ownerUser, Role.Owner.ToString());

            var adminUser = new UserModel()
            {

            };

            await _userManager.CreateAsync(adminUser, "Abc1234!");
            await _userManager.AddToRoleAsync(adminUser, Role.Admin.ToString());

            var visitorUser = new UserModel()
            {
                Email = "demovisitor@myblog.com",
                UserName = "demovisitor@myblog.com",
                FirstName = "Guest",
                LastName = "Demovisitor",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(visitorUser, "Abc1234!");
            await _userManager.AddToRoleAsync(visitorUser, Role.Visitor.ToString());

        }




    }
}
