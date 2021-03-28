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
    }
}
