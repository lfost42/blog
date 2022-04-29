using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BlogLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using BlogLibrary.Databases.Interfaces;
using BlogLibrary.Databases;
using Microsoft.Extensions.Configuration;
using System.IO;
using BlogLibrary.Models.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IBlogEmailService _emailSender;
        private readonly IImageService _imageService;
        private readonly IConfiguration _config;

        public RegisterModel(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            ILogger<RegisterModel> logger,
            IBlogEmailService emailSender,
            IImageService imageService, 
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _imageService = imageService;
            _config = config;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            public string FullName => $"{FirstName} {LastName}";

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "The {0} must be at least " +
                "{1} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            [RegularExpression("(?=.*\\d)(?=.*[!@#$%^&*]+)" +
                "(?![.\n])(?=.*[A-Z])(?=.*[a-z]).{6,}$", ErrorMessage =
                "Passwords must be at least 6 characters and contain at " +
                "least 1 of each of the following: upper case (A-Z), lower" +
                " case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Avatar")]
            [DataType(DataType.Upload)]
            [NotMapped]
            public IFormFile ImageIform { get; set; }
            public byte[] ImageData { get; set; }
            public string ContentType { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                UserModel user = new()
                {
                    Email = Input.Email,
                    UserName = Input.Email,
                    ImageData = await _imageService.EncodeImageAsync(Input.ImageIform) ??
                        await _imageService.EncodeImageAsync(_config["DefaultUserImage"]),
                    ContentType = _imageService.ContentType(Input.ImageIform) ??
                        Path.GetExtension(_config["DefaultUserImage"])
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
