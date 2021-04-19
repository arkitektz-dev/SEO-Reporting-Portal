using System.Collections.Generic;

namespace SEO_Reporting_Portal.Dtos.GeneralInquiry
{
    public class GeneralInquiryUserDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public GeneralInquiryDto RecentMessage { get; set; }

        public List<GeneralInquiryDto> Messages { get; set; }

        public GeneralInquiryUserDto()
        {
            Messages = new List<GeneralInquiryDto>();
        }
    }
}
