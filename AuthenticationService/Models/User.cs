using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class User
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string Password { get; set; }
        public string VerificationToken { get; set; }
    }
}
