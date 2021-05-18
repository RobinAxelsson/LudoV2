using System.ComponentModel.DataAnnotations;

namespace LudoAPI.Controllers.Bodies
{
    public class InviteBody
    { 
        [Required(ErrorMessage = "Hostname is required")]
        public string HostAccountName { get; set; }
        
        [Required(ErrorMessage = "User Name is required")]
        public string[] Recipients { get; set; }
    }
}