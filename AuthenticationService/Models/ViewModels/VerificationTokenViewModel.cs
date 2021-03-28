using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models.ViewModels
{
    public class VerificationTokenViewModel
    {
        public LoginViewModel loginViewModel { get; set; }
        public string VerificationToken { get; set; }
    }
}
