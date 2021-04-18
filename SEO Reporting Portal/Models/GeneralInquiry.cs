using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Models
{
    public class GeneralInquiry
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Message { get; set; }

        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        public User Respondent { get; set; }

        public string RespondentId { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public GeneralInquiry()
        {
            IsDeleted = false;
            IsActive = true;
            CreatedOn = DateTime.Now;
        }
    }
}
