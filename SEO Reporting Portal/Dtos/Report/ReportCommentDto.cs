using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Dtos.Report
{
    public class ReportCommentDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Format { get; set; }

        public string UserFullName { get; set; }

        public string UserEmail { get; set; }

        public CommentDto RecentComment { get; set; }

        public List<CommentDto> Comments { get; set; }

        public ReportCommentDto()
        {
            Comments = new List<CommentDto>();
        }
    }
}
