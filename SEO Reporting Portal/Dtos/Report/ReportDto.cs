using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SEO_Reporting_Portal.Models;

namespace SEO_Reporting_Portal.Dtos.Report
{
    public class ReportDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UniqueName { get; set; }

        public string Path { get; set; }

        public string Format { get; set; }

        public string UserFullName { get; set; }

        public string UserEmail { get; set; }

        public string CreatedOn { get; set; }
    }
}
