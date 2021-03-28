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
        public UserServiceDatabaseContext(DbContextOptions<UserServiceDatabaseContext> options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        
    }
}
