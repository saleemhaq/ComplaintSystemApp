using System;
using System.Collections.Generic;
using System.Text;
using ComplaintApp.Core.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComplaintApp.EntityFrameworkCore.Repositories
{
    public class ComplaintDbContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ComplaintDbContext(DbContextOptions<ComplaintDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Since we are using the IdentityDbContext now, we need to call
            // this method on base so that this will configure the schema
            // needed for the identity framework
            base.OnModelCreating(builder);

            // We are adding configuration for the UserRole entity
            // so that our entity framework will know the relationship
            // between the User, Role, and UserRole entity because
            // a user can have one or many roles, and the roles and be
            // assign to one or many users
            builder.Entity<UserRole>(userRole =>
            {

                // The key for this entity will be made up of the UserId
                // and the RoleId
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }
    }
}
