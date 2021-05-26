using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Database_context
{
    public class AuthenticationServiceDatabaseContext : DbContext
    {
        public AuthenticationServiceDatabaseContext(DbContextOptions<AuthenticationServiceDatabaseContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbString = Environment.GetEnvironmentVariable("kwetter-db-string");
                if (string.IsNullOrWhiteSpace(dbString))
                {
                    throw new MissingFieldException("Database environment variable not found.");
                }

                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("aci_db_string").Replace("DATABASE_NAME", "UserService"));
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
