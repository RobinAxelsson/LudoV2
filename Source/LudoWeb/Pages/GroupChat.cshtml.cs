using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoWeb.Pages
{
    public class GroupChatModel : PageModel
    {
        //public IHubContext<ChatHub> _chatContext { get; }

        //public GroupChatModel(IHubContext<ChatHub> chatContext)
        //{
        //    _chatContext = chatContext;
        //}
        //public async Task OnGet(string groupname, string message)
        public void OnGet()
        {
            Debug.WriteLine("HelloWorld");
            //if (!String.IsNullOrEmpty(groupname) || !String.IsNullOrEmpty(message))
            //{
            //    await _chatContext.Clients.Group(groupname)
            //        .SendAsync("ReceiveGroupMessage", "Endpoint", groupname, message);
            //}
        }
    }
}
