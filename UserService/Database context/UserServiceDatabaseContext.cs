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
                // Change server=127.0.0.1 for local development
                optionsBuilder.UseSqlServer("server=sqlserver, 1433;user id=sa;password=Your_password123;database=UserService;");
                
            base.OnConfiguring(optionsBuilder);
        }
    }
}
