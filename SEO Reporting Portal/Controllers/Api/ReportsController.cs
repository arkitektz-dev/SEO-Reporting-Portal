using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEO_Reporting_Portal.Dtos.Report;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(ApplicationDbContext context, UserManager<User> userManager, IWebHostEnvironment environment, ILogger<ReportsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _logger = logger;
        }

        public async Task<ActionResult<ReportViewDto>> GetReports()
        {
            List<Report> reports;
            var dto = new ReportViewDto();

            if (User.IsInRole(Roles.Administrator.ToString()))
            {
                reports = await _context.Reports.OrderBy(r => r.CreatedOn).ToListAsync();
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                reports = await _context.Reports.Where(r => r.UserId == userId).OrderBy(r => r.CreatedOn).ToListAsync();
            }

            foreach (var report in reports)
            {
                var reportDto = new ReportDto
                {
                    Id = report.Id,
                    Name = report.Name,
                    UniqueName = report.UniqueName,
                    Path = report.Path,
                    Format = report.Format,
                };

                if (!dto.TimePeriods.Contains(report.CreatedOn.ToString("MMMM-yyyy")))
                {
                    dto.TimePeriods.Add(report.CreatedOn.ToString("MMMM-yyyy"));
                }

                dto.Reports.Add(reportDto);
            }

            return dto;
        }

        [HttpGet("GetReportsByTimePeriod/{timeperiod}")]
        public async Task<ActionResult<List<ReportDto>>> GetReportsByTimePeriod(string timeperiod)
        {
            List<Report> reports;
            var dto = new List<ReportDto>();
            if (User.IsInRole(Roles.Administrator.ToString()))
            {
                reports = await _context.Reports.OrderBy(r => r.CreatedOn).ToListAsync();
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                reports = await _context.Reports.Where(r => r.UserId == userId).OrderBy(r => r.CreatedOn).ToListAsync();
            }

            foreach (var report in reports)
            {
                var createdTimePeriod = report.CreatedOn.ToString("MMMM-yyyy");
                if (timeperiod == "all")
                {
                    var reportDto = new ReportDto
                    {
                        Id = report.Id,
                        Name = report.Name,
                        UniqueName = report.UniqueName,
                        Path = report.Path,
                        Format = report.Format,
                    };

                    dto.Add(reportDto);
                }
                else
                {
                    if (createdTimePeriod == timeperiod)
                    {
                        var reportDto = new ReportDto
                        {
                            Id = report.Id,
                            Name = report.Name,
                            UniqueName = report.UniqueName,
                            Path = report.Path,
                            Format = report.Format,
                        };

                        dto.Add(reportDto);
                    }
                }

            }

            return dto;
        }

        [HttpGet("DownloadReport/{reportId}")]
        public async Task<IActionResult> DownloadReport(string reportId)
        {
            var file = await _context.Reports.SingleOrDefaultAsync(r => r.UniqueName == reportId);

            var path = Path.Combine(
                _environment.WebRootPath, "Reports", file.UniqueName);
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", file.Name);
        }

        [HttpGet("GetComments")]
        public async Task<ActionResult<IEnumerable<ReportCommentDto>>> GetComments()
        {
            List<Report> reports = new List<Report>();
            var dto = new List<ReportCommentDto>();
            if (User.IsInRole(Roles.Administrator.ToString()))
            {
                reports = await _context.Reports.ToListAsync();

            }
            else if (User.IsInRole(Roles.User.ToString()))
            {
                reports = await _context.Reports.Where(r => r.UserId == _userManager.GetUserId(User)).ToListAsync();
            }

            foreach (var report in reports)
            {
                var comments = await _context.ReportComments.Where(r => r.ReportId == report.Id && r.IsActive == true && r.IsDeleted == false).Include("User").Include("Respondent").Include("Report").OrderBy(g => g.CreatedOn).ToListAsync();
                var reportDto = new ReportCommentDto
                {
                    Id = report.Id,
                    Name = report.Name,
                    Format = report.Format
                };
                if (comments.Count > 0)
                {
                    foreach (var comment in comments)
                    {
                        var commentDto = new CommentDto
                        {
                            Id = comment.Id,
                            Text = comment.Comment,
                            ReportId = comment.ReportId,
                            UserId = comment.UserId,
                            RespondentId = comment.RespondentId,
                            SentDate = comment.CreatedOn.ToString("dd-MM-yyyy"),
                            SentTime = comment.CreatedOn.ToString("hh:mm tt"),
                        };
                        reportDto.Comments.Add(commentDto);
                    }
                    var recentComment = comments.LastOrDefault();
                    if (recentComment != null)
                    {
                        reportDto.RecentComment = new CommentDto
                        {
                            Id = recentComment.Id,
                            Text = recentComment.Comment,
                            UserId = recentComment.UserId,
                            ReportId = recentComment.RespondentId,
                            RespondentId = recentComment.RespondentId,
                            SentDate = recentComment.CreatedOn.ToString("dd-MM-yyyy"),
                            SentTime = recentComment.CreatedOn.ToString("hh:mm tt"),
                        };
                    }
                }

                dto.Add(reportDto);
            }

            return dto;
        }

        [HttpGet("GetComment/{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(string id)
        {
            var comment = await _context.ReportComments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = new CommentDto
            {
                Id = comment.Id,
                Text = comment.Comment,
                UserId = comment.UserId,
                RespondentId = comment.RespondentId,
                SentDate = comment.CreatedOn.ToString("dd-MM-yyyy"),
                SentTime = comment.CreatedOn.ToString("hh:mm tt"),
            };

            return commentDto;
        }

        [HttpGet("GetCommentsByReportId/{reportId?}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByReportId(string reportId)
        {
            //if (reportId == null && User.IsInRole(Roles.User.ToString()))
            //{
            //    userId = _userManager.GetUserId(User);
            //}

            var dto = new List<CommentDto>();
            var comments = await _context.ReportComments.Where(r => r.ReportId == reportId && r.IsActive == true && r.IsDeleted == false).Include("User").Include("Respondent").Include("Report").OrderBy(g => g.CreatedOn).ToListAsync();

            if (comments.Count > 0)
            {
                foreach (var generalInquiry in comments)
                {
                    var commentDto = new CommentDto
                    {
                        Id = generalInquiry.Id,
                        Text = generalInquiry.Comment,
                        UserId = generalInquiry.UserId,
                        RespondentId = generalInquiry.RespondentId,
                        SentDate = generalInquiry.CreatedOn.ToString("dd-MM-yyyy"),
                        SentTime = generalInquiry.CreatedOn.ToString("hh:mm tt"),
                    };

                    dto.Add(commentDto);
                }
            }

            return dto;
        }

        [HttpPost("Comment")]
        public async Task<ActionResult<CommentDto>> PostComment(CommentCreateDto commentCreateDto)
        {
            try
            {
                var comment = new ReportComment
                {
                    Id = Guid.NewGuid().ToString(),
                    Comment = commentCreateDto.Text,
                };
                var userId = _userManager.GetUserId(User);
                if (User.IsInRole(Roles.Administrator.ToString()))
                {
                    var report = await _context.Reports.SingleOrDefaultAsync(r => r.Id == commentCreateDto.ReportId);
                    comment.RespondentId = userId;
                    comment.UserId = report.UserId;
                    comment.ReportId = commentCreateDto.ReportId;
                }
                else if (User.IsInRole(Roles.User.ToString()))
                {
                    comment.UserId = userId;
                }

                _context.ReportComments.Add(comment);

                await _context.SaveChangesAsync();

                var commentDto = new CommentDto
                {
                    Id = comment.Id,
                    Text = comment.Comment,
                    UserId = comment.UserId,
                    ReportId = comment.ReportId,
                    RespondentId = comment.RespondentId,
                    SentDate = comment.CreatedOn.ToString("dd-MM-yyyy"),
                    SentTime = comment.CreatedOn.ToString("hh:mm tt"),
                };

                return CreatedAtAction(nameof(GetComment), new { id = commentDto.Id }, commentDto);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
