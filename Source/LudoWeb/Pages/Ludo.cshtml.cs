using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using LudoDataAccess;
using LudoTranslation;
using LudoWeb.ChatModels;
using LudoWeb.GameClasses;
using LudoWeb.GameInterfaces;
using LudoWeb.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.Pages
{
    public class Ludo : PageModel
    {
        public Func<int, int, GameSquareViewModel> GetGameSquare { get; }
        private readonly IDatabaseManagement _dbm;
        public readonly string DefaultRegionCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
        public int XCount { get; }
        public int YCount { get; }

        public Ludo(GameNetworkManager manager, IHtmlBoardBuilder boardBuilder, IDatabaseManagement dbm)
        {
            GetGameSquare = boardBuilder.GetGameSquare;
            XCount = boardBuilder.XCount;
            YCount = boardBuilder.YCount;
            _gameNetworkManager = manager;
            _dbm = dbm;
        }
        private GameNetworkManager _gameNetworkManager { get; }

        public void OnGet(string protocol, string gameId)
        {
            /*
            Debug.WriteLine($"User entered Ludo page with/with empty parameter: {gameId}");
            if (protocol == "addRoom" && !string.IsNullOrWhiteSpace(gameId))
            {

           
            }
            if (String.IsNullOrEmpty(gameId))
            {
                gameId = Guid.NewGuid().ToString("N");
                _gameNetworkManager.AddGameRoom(gameId);
            }
            else
            {
                
            }
            */
        }

        public void OnPost(string gameId)
        {
            
        }
    }
}
