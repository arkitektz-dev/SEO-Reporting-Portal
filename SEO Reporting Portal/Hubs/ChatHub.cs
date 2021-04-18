using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SEO_Reporting_Portal.Dtos.GeneralInquiry;
using SEO_Reporting_Portal.Dtos.Report;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ChatHub(UserManager<User> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SendMessage(string userId, GeneralInquiryDto generalInquiryDto)
        {
            //await Clients.All.SendAsync("ReceiveMessage", generalInquiryDto);
            var admins = await _userManager.GetUsersInRoleAsync(Roles.Administrator.ToString());
            var userIds = new List<string>()
            {
                userId
            };

            foreach (var admin in admins)
            {
                userIds.Add(admin.Id);
            }

            await Clients.Users(userIds).SendAsync("ReceiveMessage", generalInquiryDto);
        }

        public async Task SendReportComment(string reportId, CommentDto commentDto)
        {
            var admins = await _userManager.GetUsersInRoleAsync(Roles.Administrator.ToString());
            var userIds = new List<string>();
            var report = await _context.Reports.SingleOrDefaultAsync(r => r.Id == reportId);
            foreach (var admin in admins)
            {
                userIds.Add(admin.Id);
            }
            userIds.Add(report.UserId);

            await Clients.Users(userIds).SendAsync("ReceiveReportComment", commentDto);
        }
    }
}
