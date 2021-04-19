using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Data;
using SEO_Reporting_Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<UsersController> _logger;

        public ReportsController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, ILogger<UsersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(Roles.Administrator.ToString()))
            {
                return View("AdminReports");
            }
            else
            {
                return View("UserReports");
            }
        }

        public IActionResult Inquiries()
        {
            if (User.IsInRole(Roles.Administrator.ToString()))
            {
                return View("AdminInquiries");
            }
            else
            {
                return View("UserInquiries");
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Upload()
        {
            var users = await _userManager.GetUsersInRoleAsync(Roles.User.ToString());

            var viewModel = new UploadReportViewModel
            {
                Users = users
            };

            return View("UploadForm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormCollection FormCollection)
        {
            foreach (var file in FormCollection.Files)
            {
                var name = file.FileName;
                var extension = Path.GetExtension(name);
                var nameExcludingExtension = Path.GetFileNameWithoutExtension(name);
                var uniqueName = $@"{DateTime.Now.Ticks}{extension}";
                var size = file.Length;
                var path = Path.Combine(
                 _environment.WebRootPath, "Reports",
                    uniqueName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var report = new Report()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = FormCollection["UserId"][0],
                    Path = "\\" + uniqueName,
                    UniqueName = uniqueName,
                    Name = name,
                    NameExcludingExtenstion = nameExcludingExtension,
                    Format = extension,
                    Size = size,
                    CreatedOn = DateTime.Now,
                    CreatedBy = _userManager.GetUserId(User)
                };
                _context.Add(report);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Download(string id)
        {
            var file = await _context.Reports.SingleOrDefaultAsync(r => r.Id == id);

            var path = Path.Combine(
                _environment.WebRootPath, "Reports", file.UniqueName);
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", file.Name);
        }
    }
}
