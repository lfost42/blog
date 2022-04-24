﻿using BlogLibrary.Data;
using BlogLibrary.Databases.Interfaces;
using BlogLibrary.Models;
using BlogLibrary.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogLibrary.Databases
{
    public class SeedService
    {
        private readonly BlogContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly IImageService _imageService;
        private readonly IConfiguration _config;

        public SeedService(BlogContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<UserModel> userManager, IConfiguration config, IImageService imageService)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _config = config;
            _imageService = imageService;
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
            if (_dbContext.AppUsers.Any()) return;

            string defaultPassword = _config["DefaultPassword"];
            string ownerPassword = _config["OwnerPassword"];

            var ownerUser = new UserModel()
            {
                Email = "lyndabrf@gmail.com",
                UserName = "lyndabrf@gmail.com",
                FirstName = "Lynda",
                LastName = "Foster",
                EmailConfirmed = true,
                ImageData = await _imageService.EncodeImageAsync("defaultUser.png"),
                ContentType = Path.GetExtension(_config["defaultUser.png"])
            };

            if (_dbContext.Series.Any()) return;
            await _userManager.CreateAsync(ownerUser, "ABC!123");
            await _userManager.AddToRoleAsync(ownerUser, Role.Owner.ToString());
        }

    }

}
