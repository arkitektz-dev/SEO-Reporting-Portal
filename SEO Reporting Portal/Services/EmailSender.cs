using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public virtual Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = _config.GetValue<string>(
                "MailSettings:Mail");
            var password = _config.GetValue<string>(
               "MailSettings:Password");
            var host = _config.GetValue<string>(
               "MailSettings:Host");
            var port = _config.GetValue<int>(
               "MailSettings:Port");
            var ssl = _config.GetValue<bool>(
            "MailSettings:ssl");
            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(mail, password),
                UseDefaultCredentials = false,
                EnableSsl = ssl,
            };
            return client.SendMailAsync(
                new MailMessage(mail, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}
