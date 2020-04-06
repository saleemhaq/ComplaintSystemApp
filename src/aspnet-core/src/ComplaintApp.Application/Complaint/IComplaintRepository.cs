using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ComplaintApp.API.Helpers;
using ComplaintApp.Application.Helpers;
using ComplaintApp.Core.Users;

namespace ComplaintApp.Application.Complaint
{
    public interface IComplaintRepository
    {
        void Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        Task<bool> SaveAll();

        Task<PagedList<User>> GetUsers(UserParams userParams);

        Task<User> GetUser(int id, bool isCurrentUser);

    }
}
