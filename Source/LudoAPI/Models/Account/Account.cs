using System.ComponentModel.DataAnnotations;
using LudoAPI.Translation;

namespace LudoAPI.Models.Account
{
    public record Account
    {
        public int Id { get; set; }
        [Required]
        public string PlayerName { get; set; }
        [Required]
        public string EmailAdress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public TranslationEngine.Language Language { get; set; }
        public string Role { get; set; }
    }
}