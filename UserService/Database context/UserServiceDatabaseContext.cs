using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService
{
    public class UserServiceDatabaseContext: DbContext
    {
        public UserServiceDatabaseContext()
        {

        }
        public UserServiceDatabaseContext(DbContextOptions<UserServiceDatabaseContext> options):base(options)
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
