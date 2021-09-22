using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username cannot be empty!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password cannot be empty!")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
