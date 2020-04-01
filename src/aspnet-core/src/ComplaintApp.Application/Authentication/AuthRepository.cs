using ComplaintApp.Core.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ComplaintApp.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ComplaintApp.Application.Authentication
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ComplaintDbContext _context;

        public AuthRepository(ComplaintDbContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(f => f.UserName.Equals(username.Trim(),
                    StringComparison.InvariantCultureIgnoreCase));

            if (user == null)
                return null;

            return null;
        }
    }
}
