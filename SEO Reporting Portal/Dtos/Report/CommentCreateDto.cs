using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Dtos.Report
{
    public class CommentCreateDto
    {
        public string Text { get; set; }
        public string ReportId { get; set; }
    }
}
