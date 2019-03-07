using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class RegistrationModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters long.")]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password confirmation invalid. Please input password again."), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}