using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ComplaintApp.Application.Complaint;
using ComplaintApp.Application.Dtos;
using ComplaintApp.Application.Shared;
using ComplaintApp.Core.Complaint;
using ComplaintApp.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace ComplaintApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : Controller
    {
        private readonly IComplaintRepository _ComplaintRepository;
        private readonly ComplaintDbContext _context;

        public ComplaintController(IComplaintRepository complaintRepository, ComplaintDbContext context)
        {
            _ComplaintRepository = complaintRepository;
            _context = context;
        }

        [HttpGet("getComplaints")]
        public async Task<IActionResult> GetComplaints()
        {
            var complaints = await (from complaint in _context.Complaints where complaint.IsActive == true select  complaint).ToListAsync();
            foreach (var item in complaints)
            {
                switch (item.Status)
                {
                    case "Pending":
                        item.Status = "30";;
                        break;
                    case "Open":
                        item.Status = "40";
                        break;
                    case "In Progress":
                        item.Status = "60";
                        break;
                    case "Done":
                        item.Status = "100";
                        break;
                }
            }
            return Ok(complaints);
        }

        [HttpGet("getComplaintById")]
        public async Task<IActionResult> GetComplaintById(int id)
        {
            var complaint =await _context.Complaints.Where(x => x.Id == id).FirstOrDefaultAsync();
            return Ok(complaint);
        }

        [HttpPost("registerComplaint")]
        public async Task<IActionResult> RegisterComplaint(ComplaintDto complaintDtoInput)
        {
            try
            {
                var complaint = new Complaint
                {
                    CreationTime = DateTime.UtcNow,
                    ComplaintName = complaintDtoInput.ComplaintName,
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


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComplaint([FromRoute] int id,
            [FromBody] ComplaintDto complaintForUpdateDto)
        {
            // The first thing that we want to do is to check the user that is
            // attempting to update their profile matches the token that the
            // service is receiving. On the AuthController at line #77, we are
            // setting the ClaimTypes.NameIdentifier equal to the user identifier
            //if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //    return Unauthorized();

            var getComplaint = await _context.Complaints.Where(x=>x.Id == id).FirstOrDefaultAsync();

            getComplaint.ComplaintName = complaintForUpdateDto.ComplaintName;
            //getComplaint.City = complaintForUpdateDto.City;
            //getComplaint.Country= complaintForUpdateDto.Country;
            //getComplaint.Email= complaintForUpdateDto.;
            //getComplaint.ComplaintRegarding= complaintForUpdateDto.ComplaintRegarding;
            getComplaint.Status= complaintForUpdateDto.Status;
            getComplaint.UpdatedTime = DateTime.UtcNow;
            getComplaint.UpdatedBy = 8;
            _context.Entry(getComplaint).State = EntityState.Modified;
            _context.SaveChanges();

            //// If this is successful, then we will just return NoContent();
            //if (await _ComplaintRepository.SaveAll())
            //    return NoContent();
            return NoContent();

            // Get the information from the userForUpdateDto and map it to 
            // the userFromRepo
            //_mapper.Map(userForUpdateDto, userFromRepo);

            //// If this is successful, then we will just return NoContent();
            //if (await _repo.SaveAll())
            //    return NoContent();

            //throw new Exception($"Updating user {id} failed on saved.");
        }
    }
}