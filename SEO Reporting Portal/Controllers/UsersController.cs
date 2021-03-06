using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Data;
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
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<UsersController> _logger;
        private static string _userId;
        private readonly ApplicationDbContext _context;

        public UsersController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService,
            ILogger<UsersController> logger
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {

            var companyList = _context.Company.ToList();

            var model = new UserFormViewModel()
            {
                Companys = companyList
            };

            return View("UserForm", model);
        }

        public IActionResult Edit(string UserId)
        {

            var UserDetail = _context.Users.Where(x => x.Id == UserId).FirstOrDefault();

            var model = new UserFormViewModel()
            {
                FullName = UserDetail.FullName,
                Email = UserDetail.Email,
                ContractStartDate = UserDetail.ContractStartDate,
                ContractEndDate = UserDetail.ContractEndDate,
                Status = UserDetail.Status,
                Companys = _context.Company.ToList(),
                CompanyId = UserDetail.CompanyId,
                UserId = UserDetail.Id
             };



            return View("EditUserForm", model);
        }

        [HttpPost]
        public IActionResult EditUser(UserFormViewModel model)
        {

            if (ModelState.IsValid)
            {

                var userDetail = _context.Users.Where(x => x.Id == model.UserId).FirstOrDefault();

                userDetail.FullName = model.FullName;
                userDetail.Email = model.Email;
                userDetail.ContractStartDate = model.ContractStartDate;
                userDetail.ContractEndDate = model.ContractEndDate;
                userDetail.CompanyId = model.CompanyId;
                userDetail.Status = model.Status;

                _context.SaveChanges();

                var companyUserRow = new CompanyUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    CompanyId = model.CompanyId,
                    UserId = model.UserId,
                    CreatedDate = DateTime.Now.Date

                };

                _context.CompanyUsers.Add(companyUserRow);
                _context.SaveChanges();


                return RedirectToAction("Index", "Users");

            }


            // If we got this far, something failed, redisplay form
            return View("EditUserForm", model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { FullName = model.FullName, UserName = model.Email, Email = model.Email, ContractStartDate = model.ContractStartDate, ContractEndDate = model.ContractEndDate, Status = model.Status, CompanyId = model.CompanyId };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    ViewBag.UserList = await _userManager.GetUsersInRoleAsync(Roles.User.ToString());
 
                     

                    var roleResult = await _userManager.AddToRoleAsync(user, Roles.User.ToString());

                    if (roleResult.Succeeded)
                    {
                        _logger.LogInformation("User Role created.");

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Action("SetPassword", "Account", values: new { userId = user.Id, code }, Request.Scheme);

                        _ = _emailService.SendAsync(model.Email, "Confirm your email",
                      $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.", true);

                        return RedirectToAction("Index", "Users");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.Companys = _context.Company.ToList();

            // If we got this far, something failed, redisplay form
            return View("UserForm", model);
        }




        public async Task<IActionResult> SetPassword(string id)
        {
            if (id == null)
            {
                return BadRequest("user id must be supplied to set password.");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'.");
            }

            _userId = id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByIdAsync(_userId);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
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

                _userId = null;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Some Error Occured");
            return View(model);
        }

    }
}
