using SEO_Reporting_Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string ContractStartDate { get; set; }

        public string ContractEndDate { get; set; }

        public string Status { get; set; }
    }
}
