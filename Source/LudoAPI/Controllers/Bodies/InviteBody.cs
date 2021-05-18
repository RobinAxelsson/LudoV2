using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LudoAPI.Controllers.Bodies
{
    public class InviteBody
    { 
        [Required(ErrorMessage = "Hostname is required")]
        public string HostAccountName { get; set; }
        public string[] Recipients { get; set; }
    }
}