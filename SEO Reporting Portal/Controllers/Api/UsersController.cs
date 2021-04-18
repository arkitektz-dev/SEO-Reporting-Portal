using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEO_Reporting_Portal.Dtos.User;
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
    [Authorize(Roles = "Administrator")]

    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;
        public UsersController(ApplicationDbContext context, UserManager<User> userManager, ILogger<UsersController> logger)
        {
            _context = context;
            _userManager = userManager;
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
                    ContractStartDate = user.ContractStartDate.Value,
                    ContractEndDate = user.ContractEndDate.Value,
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
    }
}
