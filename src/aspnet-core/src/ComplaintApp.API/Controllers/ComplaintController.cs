using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComplaintApp.Application.Complaint;
using ComplaintApp.Application.Dtos;
using ComplaintApp.Core.Complaint;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ComplaintApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : Controller
    {
        private readonly IComplaintRepository _ComplaintRepository;

        public ComplaintController(IComplaintRepository complaintRepository)
        {
            _ComplaintRepository = complaintRepository;
        }

        [HttpPost("registerComplaint")]
        public async Task<IActionResult> RegisterComplaint(ComplaintDto complaintDtoInput)
        {
            try
            {
                var complaint = new Complaint
                {
                    CreationTime = DateTime.UtcNow,
                    ComplainantName = complaintDtoInput.ComplaintName,
                    ComplaintRegarding = complaintDtoInput.ComplaintRegarding,
                    Description = complaintDtoInput.Description,
                    Email = complaintDtoInput.Email,
                    IsActive = true,
                    IsDeleted = false,
                    CreatorUserId = 8
                };
                _ComplaintRepository.Add(complaint);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, Please try again.");
            }
        }
    }
}