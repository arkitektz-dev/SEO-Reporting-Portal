using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Validator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.ViewModels
{
    public class UserFormViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DateShouldBeGreaterThanCurrentDate]
        [Display(Name = "Contract Start Date")]
        public DateTime? ContractStartDate { get; set; }

        [Required]
        [DateShouldBeGreaterThanSpecificDate]
        [Display(Name = "Contract End Date")]
        public DateTime? ContractEndDate { get; set; }

        [Required]
        [Display(Name = "Account Status")]
        public AccountStatus Status { get; set; }

    }
}
