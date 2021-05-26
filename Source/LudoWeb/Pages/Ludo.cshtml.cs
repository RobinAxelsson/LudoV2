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

        public Ludo(GameNetworkManager manager, IHtmlBoardBuilder boardBuilder)
        {
            GetGameSquare = boardBuilder.GetGameSquare;
            XCount = boardBuilder.XCount;
            YCount = boardBuilder.YCount;
            _gameNetworkManager = manager;
        }
        private GameNetworkManager _gameNetworkManager { get; }

        public void OnGet(string gameId)
        {
            Debug.WriteLine($"User entered Ludo page with/with empty parameter: {gameId}");
            if (String.IsNullOrEmpty(gameId))
            {
                gameId = Guid.NewGuid().ToString("N");
                _gameNetworkManager.AddGameRoom(gameId);
            }
            else
            {
                
            }
        }
    }
}
