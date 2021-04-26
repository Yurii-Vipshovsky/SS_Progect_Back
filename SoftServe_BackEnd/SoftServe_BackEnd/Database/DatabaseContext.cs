using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SoftServe_BackEnd.Models;

namespace SoftServe_BackEnd.Database
{
    public class DatabaseContext: IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options)
        {

        }
        public DbSet<Event> Events { get; set; }

        public DbSet<IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }
        public DbSet<IdentityUserClaim<string>> IdentityUserClaim { get; set; }
    }
}