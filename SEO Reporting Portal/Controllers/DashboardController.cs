using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Data;
using SEO_Reporting_Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ApplicationDbContext context, UserManager<User> userManager, ILogger<DashboardController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Roles.Administrator.ToString()))
            {
                var totalUsers = await _userManager.GetUsersInRoleAsync(Roles.User.ToString());
                var totalReports = _context.Reports.Count();
                var totalGeneralInquiries = _context.GeneralInquiries.Count();
                var totalReportComments = _context.ReportComments.Count();
                var adminViewModel = new AdminDashboardViewModel()
                {
                    TotalUsers = totalUsers.Count,
                    TotalReports = totalReports,
                    TotalGeneralInquiries = totalGeneralInquiries,
                    TotalReportComments = totalReportComments
                };

                return View("AdminDashboard", adminViewModel);
            }

            var user = await _userManager.GetUserAsync(User);
            var currentDateTime = DateTime.Now;
            var currentMonth = currentDateTime.Month;
            var currentYear = currentDateTime.Year;
            var currentDay = currentDateTime.Day;
            var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);

            var userViewModel = new UserDashboardViewModel()
            {
                ContractStartDate = user.ContractStartDate.Value.ToString("dd-MMM-yyyy"),
                ContractEndDate = user.ContractEndDate.Value.ToString("dd-MMM-yyyy")
            };

            var userReports = await _context.Reports.Where(r => r.UserId == user.Id).OrderByDescending(r => r.CreatedOn).ToListAsync();

            var days = new DateTime(currentYear, currentMonth, currentDay).Subtract(userReports.Count == 0 ? user.ContractStartDate.Value : userReports[0].CreatedOn).Days;
            userViewModel.TotalReports = userReports.Count;
            userViewModel.UpcomingReportDays = days > 0 ? daysInMonth - days : 0;

            return View("UserDashboard", userViewModel);
        }
    }
}
