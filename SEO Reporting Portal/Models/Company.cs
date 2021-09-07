using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Models
{
    public class Company
    {
        public string Id { get; set; }
        public string Name { get; set;}
        public string PhoneNumber { get; set; }
        public string WebsiteUrl { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
