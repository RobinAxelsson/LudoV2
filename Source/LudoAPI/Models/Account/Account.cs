using System.ComponentModel.DataAnnotations;

namespace LudoAPI.Models.Account
{
    public record Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PlayerName { get; set; }
        [Required]
        public string EmailAdress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Language { get; set; }
        public string Role { get; set; }
    }
}