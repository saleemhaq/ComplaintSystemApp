using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComplaintApp.Application.Shared;
using ComplaintApp.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplaintApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ComplaintDbContext _context;

        public DashboardController(ComplaintDbContext context)
        {
            _context = context;
        }

        [Route("GetCharts")]
        [HttpGet]
        public async Task<IActionResult> GetChartsData()
        {
            var complaints = await(from complaint in _context.Complaints select complaint).ToListAsync();
            return Ok(complaints);
        }
    }
}