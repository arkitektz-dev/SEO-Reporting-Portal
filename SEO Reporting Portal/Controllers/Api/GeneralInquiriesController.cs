using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEO_Reporting_Portal.Dtos.GeneralInquiry;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GeneralInquiriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<GeneralInquiriesController> _logger;
        public GeneralInquiriesController(ApplicationDbContext context, UserManager<User> userManager, ILogger<GeneralInquiriesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeneralInquiryUserDto>>> GetGeneralInquiries()
        {
            var dto = new List<GeneralInquiryUserDto>();
            var users = await _userManager.GetUsersInRoleAsync(Roles.User.ToString());
            foreach (var user in users)
            {
                var generalInquiries = await _context.GeneralInquiries.Where(u => u.UserId == user.Id && u.IsActive == true && u.IsDeleted == false).Include("User").Include("Respondent").OrderBy(g => g.CreatedOn).ToListAsync();
                var userDto = new GeneralInquiryUserDto
                {
                    Id = user.Id,
                    FullName = user.FullName
                };
                if (generalInquiries.Count > 0)
                {
                    foreach (var generalInquiry in generalInquiries)
                    {
                        var messageDto = new GeneralInquiryDto
                        {
                            Id = generalInquiry.Id,
                            Message = generalInquiry.Message,
                            UserId = generalInquiry.UserId,
                            RespondentId = generalInquiry.RespondentId,
                            SentDate = generalInquiry.CreatedOn.ToString("dd-MM-yyyy"),
                            SentTime = generalInquiry.CreatedOn.ToString("hh:mm tt"),
                        };
                        userDto.Messages.Add(messageDto);
                    }
                    var recentMessage = generalInquiries.LastOrDefault();
                    if (recentMessage != null)
                    {
                        userDto.RecentMessage = new GeneralInquiryDto
                        {
                            Id = recentMessage.Id,
                            Message = recentMessage.Message,
                            UserId = recentMessage.UserId,
                            RespondentId = recentMessage.RespondentId,
                            SentDate = recentMessage.CreatedOn.ToString("dd-MM-yyyy"),
                            SentTime = recentMessage.CreatedOn.ToString("hh:mm tt"),
                        };
                    }
                }

                dto.Add(userDto);
            }

            return dto;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneralInquiryDto>> GetGeneralInquiry(string id)
        {
            var generalInquiry = await _context.GeneralInquiries.FindAsync(id);

            if (generalInquiry == null)
            {
                return NotFound();
            }

            var generalInquiryDto = new GeneralInquiryDto
            {
                Id = generalInquiry.Id,
                Message = generalInquiry.Message,
                UserId = generalInquiry.UserId,
                RespondentId = generalInquiry.RespondentId,
                SentDate = generalInquiry.CreatedOn.ToString("dd-MM-yyyy"),
                SentTime = generalInquiry.CreatedOn.ToString("hh:mm tt"),
            };

            return generalInquiryDto;
        }

        [HttpGet("GetGeneralInquiriesByUserId/{userId?}")]
        public async Task<ActionResult<IEnumerable<GeneralInquiryDto>>> GetGeneralInquiriesByUserId(string userId)
        {
            if (userId == null && User.IsInRole(Roles.User.ToString()))
            {
                userId = _userManager.GetUserId(User);
            }

            var dto = new List<GeneralInquiryDto>();
            var generalInquiries = await _context.GeneralInquiries.Where(u => u.UserId == userId && u.IsActive == true && u.IsDeleted == false).Include("User").Include("Respondent").OrderBy(g => g.CreatedOn).ToListAsync();
            if (generalInquiries.Count > 0)
            {
                foreach (var generalInquiry in generalInquiries)
                {
                    var messageDto = new GeneralInquiryDto
                    {
                        Id = generalInquiry.Id,
                        Message = generalInquiry.Message,
                        UserId = generalInquiry.UserId,
                        RespondentId = generalInquiry.RespondentId,
                        SentDate = generalInquiry.CreatedOn.ToString("dd-MM-yyyy"),
                        SentTime = generalInquiry.CreatedOn.ToString("hh:mm tt"),
                    };

                    dto.Add(messageDto);
                }
            }

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<GeneralInquiryDto>> PostGeneralInquiry(GeneralInquiryCreateDto generalInquiryCreateDto)
        {
            var generalInquiry = new GeneralInquiry
            {
                Id = Guid.NewGuid().ToString(),
                Message = generalInquiryCreateDto.Message,
            };

            if (User.IsInRole(Roles.Administrator.ToString()))
            {
                generalInquiry.RespondentId = _userManager.GetUserId(User);
                generalInquiry.UserId = generalInquiryCreateDto.UserId;
            }
            else if (User.IsInRole(Roles.User.ToString()))
            {
                generalInquiry.UserId = _userManager.GetUserId(User);
            }

            _context.GeneralInquiries.Add(generalInquiry);

            await _context.SaveChangesAsync();

            var generalInquiryDto = new GeneralInquiryDto
            {
                Id = generalInquiry.Id,
                Message = generalInquiry.Message,
                UserId = generalInquiry.UserId,
                RespondentId = generalInquiry.RespondentId,
                SentDate = generalInquiry.CreatedOn.ToString("dd-MM-yyyy"),
                SentTime = generalInquiry.CreatedOn.ToString("hh:mm tt"),
            };

            return CreatedAtAction(nameof(GetGeneralInquiry), new { id = generalInquiryDto.Id }, generalInquiryDto);
        }

        //    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        //new TodoItemDTO
        //{
        //    Id = todoItem.Id,
        //    Name = todoItem.Name,
        //    IsComplete = todoItem.IsComplete
        //};
    }
}
