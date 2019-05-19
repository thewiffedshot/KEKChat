using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class EditUserModel
    {
        [Required]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 16 characters long.")]
        [RegularExpression("^[a-zA-Z_]([a-zA-Z0-9_]+)$", ErrorMessage = "Username must contain only english letters, numbers and '_'")]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password confirmation invalid. Please input password again."), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}