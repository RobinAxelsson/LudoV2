using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LudoWeb.ChatModels;
using LudoWeb.GameClasses;
using LudoWeb.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.Pages
{
    public class Ludo : PageModel
    {
        public Func<int, int, GameSquareViewModel> GetGameSquare { get; }
        public int XCount { get; }
        public int YCount { get; }

        public Ludo(IHubContext<ChatHub> chatContext, IHtmlBoardBuilder boardBuilder)
        {
            GetGameSquare = boardBuilder.GetGameSquare;
            XCount = boardBuilder.XCount;
            YCount = boardBuilder.YCount;
            _chatContext = chatContext;
        }
        public IHubContext<ChatHub> _chatContext { get; }

        public async Task OnGetAsync(string group, string message)
        {
            Debug.WriteLine(group + ": " + message);
            if (!String.IsNullOrEmpty(group) || !String.IsNullOrEmpty(message))
            {
                await _chatContext.Clients.Group(group)
                    .SendAsync("ReceiveGroupMessage", "Endpoint", group, message);
            }

        }
    }
}
