using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComplaintApp.API.Helpers;
using ComplaintApp.Application.Helpers;
using ComplaintApp.Core.Users;
using ComplaintApp.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ComplaintApp.Application.Complaint
{
    public class ComplaintRepository : IComplaintRepository
    {
        #region Fields

        private readonly ComplaintDbContext _context;

        #endregion

        #region Ctor

        public ComplaintRepository(ComplaintDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {

            var users = _context.Users
                .Where(w => w.Id != userParams.UserId);



            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.CreationTime);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.CreationTime);
                        break;
                }
            }
            return await PagedList<User>.CreateAsync(users,
                userParams.PageNumber, userParams.PageSize);
        }

        public async Task<User> GetUser(int id, bool isCurrentUser)
        {
            var query = _context.Users.AsQueryable();

            // In our DataContext.OnModelCreating method, we are adding the global filter
            // builder.Entity<Photo>().HasQueryFilter(p => p.IsApproved);
            // to only return the photos that are approved.
            // So, we need to check if it is the current user.  If true, then we will
            // need to call the .IgnoreQueryFilter() so that we can
            // send all the photos regardless if it is approved or not.
            if (isCurrentUser)
                query = query.IgnoreQueryFilters();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user; ;
        }
        #endregion
    }
}
