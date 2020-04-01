using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ComplaintApp.Core.Users
{
    // By using IdentityRole<int>, we are now specifying that the primary key for the role
    // is of type int and not a string.
    public class Role : IdentityRole<int>
    {
        #region Navigation properties

        public ICollection<UserRole> UserRoles { get; set; }

        #endregion
    }
}
