using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.ViewModels
{
    public class ForgotPasswordViewModel
    {
        public string StatusMessage { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
