using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ComplaintApp.Core.Users;

namespace ComplaintApp.Application.Authentication
{
    public interface IAuthRepository
    {
        Task<User> Login(string username, string password);
    }
}
