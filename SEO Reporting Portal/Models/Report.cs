using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Models
{
    public class Report
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} at max {1} characters long.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} at max {1} characters long..")]
        public string NameExcludingExtenstion { get; set; }

        [Required]
        public string UniqueName { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "The {0} at max {1} characters long.")]
        public string Format { get; set; }

        [Required]
        public long Size { get; set; }

        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string DeletedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public Report()
        {
            IsDeleted = false;
            IsActive = true;
            CreatedOn = DateTime.Now;
        }
    }
}
