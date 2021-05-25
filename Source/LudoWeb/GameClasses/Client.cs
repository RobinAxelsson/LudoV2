using LudoAPI.Models;
using LudoAPI.Models.Account;

namespace LudoWeb.GameClasses
{
    public class Client
    {
        public Client(string connectionId)
        {
            ConnectionId = connectionId;
        }
        public string ConnectionId { get; set; }
        public string ChatName { get; set; }
        public Account Account { get; set; }
        public Player Player { get; set; }
    }
}