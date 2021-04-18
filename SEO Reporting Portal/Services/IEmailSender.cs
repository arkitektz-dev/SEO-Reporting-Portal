using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
