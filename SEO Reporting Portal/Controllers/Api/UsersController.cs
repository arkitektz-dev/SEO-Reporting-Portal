using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEO_Reporting_Portal.Dtos.User;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Data;
using SEO_Reporting_Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]

    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UsersController> _logger;
        public UsersController(ApplicationDbContext context, UserManager<User> userManager, IEmailSender emailSender, ILogger<UsersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var usersDto = new List<UserDto>();
            var users = await _userManager.GetUsersInRoleAsync(Roles.User.ToString());
            foreach (var user in users)
            {
                var userDto = new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    ContractStartDate = user.ContractStartDate.Value.ToString("dd-MMM-yyyy"),
                    ContractEndDate = user.ContractEndDate.Value.ToString("dd-MMM-yyyy"),
                    Status = user.Status.ToString()
                };

                usersDto.Add(userDto);
            }

            return usersDto;
        }

        [HttpPut("UpdateUserStatus/{userId}")]
        public async Task<IActionResult> UpdateUserStatus(string userId, UpdateUserStatusDto updateUserStatusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userInDb = await _userManager.FindByIdAsync(userId);

            if (userInDb == null)
                return NotFound();

            _ = Enum.TryParse(updateUserStatusDto.Status, out AccountStatus accountStatus);
            userInDb.Status = accountStatus;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationDto resendEmailConfirmationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(resendEmailConfirmationDto.UserId);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
                return NotFound();
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("SetPassword", "Account", values: new { userId = user.Id, code }, Request.Scheme);
            _ = _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");
            return Ok();
        }
    }
}
