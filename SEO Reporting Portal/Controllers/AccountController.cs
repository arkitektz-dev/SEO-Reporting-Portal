using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Services;
using SEO_Reporting_Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;
        private static string _code;
        private static string _userId;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, IEmailSender emailSender, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        if (user.Status == AccountStatus.Active)
                        {
                            _logger.LogInformation("User logged in.");
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else if (user.Status == AccountStatus.Deleted)
                        {
                            _logger.LogWarning("User account Deleted.");
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View(model);
                        }
                        else if (user.Status == AccountStatus.Suspended)
                        {
                            _logger.LogWarning("User account Suspended.");
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return View(model);
                        }
                    }

                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> SetPassword(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("A code must be supplied to set password.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            _code = code;
            _userId = userId;

            return View();
        }

        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByIdAsync(_userId);

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(_code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!addPasswordResult.Succeeded)
                {
                    foreach (var error in addPasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction("SetPassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("ChangePassword", model);
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User changed their password successfully.");

            return RedirectToAction("Login");
        }

        public IActionResult ForgotPassword()
        {
            var viewModel = new ForgotPasswordViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var viewModel = new ForgotPasswordViewModel();
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    model.StatusMessage = "Some Error Occured";
                    return View(model);
                }
                if (user.Status != AccountStatus.Active)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    model.StatusMessage = "Your account in not active";
                    return View(model);
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ResetPassword", "Account", values: new { code }, Request.Scheme);

                _ = _emailSender.SendEmailAsync(model.Email, "Reset Password",
                 $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                model.StatusMessage = "Reset Link is send to your email";

                return View(model);
            }

            model.StatusMessage = "Some Error Occured";

            return View(model);
        }

        public IActionResult ResetPassword(string code = null)
        {
            var viewModel = new ResetPasswordViewModel();

            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                _code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                return View("ResetPassword", viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                model.StatusMessage = "Some Error Occured";

                return View(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, _code, model.Password);
            if (result.Succeeded)
            {
                _code = null;
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

    }
}
