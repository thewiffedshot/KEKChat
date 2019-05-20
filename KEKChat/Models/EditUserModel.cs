using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class EditUserModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(language_strings))]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Username must be between 4 and 16 characters long.")]
        [RegularExpression("^[a-zA-Z_]([a-zA-Z0-9_]+)$", ErrorMessageResourceName = "UsernameInvalidError", ErrorMessageResourceType = typeof(language_strings))]
        [Display(Name = "UsernameFieldLabel", ResourceType = typeof(language_strings))]
        public string Username { get; set; }

        [RegularExpression("^([a-zA-Z0-9_\\.]+)$", ErrorMessageResourceName = "EmailInvalidError", ErrorMessageResourceType = typeof(language_strings))]
        [Display(Name = "EmailFieldLabel", ResourceType = typeof(language_strings))]
        [UIHint("Email")]
        public string EmailName { get; set; } = "";

        public string EmailDomain { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(language_strings))]
        [MinLength(6, ErrorMessageResourceName = "PasswordLengthError", ErrorMessageResourceType = typeof(language_strings))]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordFieldLabel", ResourceType = typeof(language_strings))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(language_strings))]
        [Compare("Password", ErrorMessageResourceName = "RequiredFieldError", ErrorMessageResourceType = typeof(language_strings))]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPasswordFieldLabel", ResourceType = typeof(language_strings))]
        public string ConfirmPassword { get; set; }
    }
}