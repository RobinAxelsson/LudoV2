using LudoDataAccess.Models;
using LudoDataAccess.Models.Account;

namespace LudoWeb.GameClasses
{
    public class Client
    {
        public Client(string connectionId)
        {
            ConnectionId = connectionId;
        }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public Player Player { get; set; }
    }
}