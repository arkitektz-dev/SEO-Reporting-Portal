using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.ViewModels
{
    public class UserDashboardViewModel
    {
        public int TotalReports { get; set; }
        public int UpcomingReportDays { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
    }
}
