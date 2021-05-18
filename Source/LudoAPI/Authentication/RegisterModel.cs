using System.ComponentModel.DataAnnotations;

namespace LudoAPI.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Account name is required")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Preferred language is required")]
        public string PreferredLanguage { get; set; }
    }
}
