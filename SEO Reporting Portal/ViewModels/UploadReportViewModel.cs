using SEO_Reporting_Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.ViewModels
{
    public class UploadReportViewModel
    {
        public IEnumerable<User> Users { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }
    }
}
