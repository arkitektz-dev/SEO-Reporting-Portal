using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Dtos.Report
{
    public class CommentDto
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string ReportId { get; set; }

        public string UserId { get; set; }

        public string RespondentId { get; set; }

        public string SentDate { get; set; }

        public string SentTime { get; set; }
    }
}
