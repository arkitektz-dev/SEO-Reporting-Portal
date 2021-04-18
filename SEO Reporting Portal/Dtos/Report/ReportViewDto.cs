using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Dtos.Report
{
    public class ReportViewDto
    {
        public List<ReportDto> Reports { get; set; }

        public List<string> TimePeriods { get; set; }

        public ReportViewDto()
        {
            Reports = new List<ReportDto>();
            TimePeriods = new List<string>();
        }
    }
}
