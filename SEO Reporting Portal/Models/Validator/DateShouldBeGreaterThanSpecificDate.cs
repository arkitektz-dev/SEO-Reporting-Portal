using SEO_Reporting_Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Models.Validator
{
    public class DateShouldBeGreaterThanSpecificDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (UserFormViewModel)validationContext.ObjectInstance;
            if (user.ContractStartDate == null)
            {
                return new ValidationResult("Contract Start Date is required");
            }
            if (user.ContractEndDate == null)
            {
                return new ValidationResult("Contract End Date is required");
            }

            var result = DateTime.Compare(user.ContractEndDate.Value, user.ContractStartDate.Value);

            if (result > 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Contract end date should be greater than contract start date");
            }

        }
    }
}
