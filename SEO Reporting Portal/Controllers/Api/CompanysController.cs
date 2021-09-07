using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEO_Reporting_Portal.Dtos.Company;
using SEO_Reporting_Portal.Models;
using SEO_Reporting_Portal.Models.Data;
using SEO_Reporting_Portal.Services;
using SEO_Reporting_Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEO_Reporting_Portal.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class CompanysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UsersController> _logger;
        public CompanysController(ApplicationDbContext context, UserManager<User> userManager, IEmailSender emailSender, ILogger<UsersController> logger)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }


        public async Task<ActionResult<List<CompanyDto>>> GetCompanys()
        {
            var companyDto = new List<CompanyDto>();
            var companys = await _context.Company.ToListAsync();
            foreach (var company in companys)
            {
                var userDto = new CompanyDto
                {
                   Id = company.Id,
                   Name = company.Name,
                   PhoneNumber = company.PhoneNumber,
                   WebsiteUrl = company.WebsiteUrl
                };

                companyDto.Add(userDto);
            }

            return companyDto;
        }

      

        [HttpGet("DeleteCompany")]
        public async Task<ActionResult<CompanyDto>> DeleteCompany(string companyId)
        {
            try
            {


                var company = _context.Company.SingleOrDefault(r => r.Id == companyId);

                if (company == null)
                {
                    return NotFound("Company not found");
                }

                //check if company is assigned to user table
                var checkcompanyname = _context.Users.Where(x => x.CompanyId == companyId).FirstOrDefault();

                if (checkcompanyname != null) {
                    return BadRequest("a user has already assosiated with this company");
                }
 


                _context.Company.Remove(company);
                await _context.SaveChangesAsync();
                 
                return Ok(companyId);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
