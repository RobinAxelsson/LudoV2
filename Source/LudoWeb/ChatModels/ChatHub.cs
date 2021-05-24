using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

//https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/working-with-groups#call
namespace LudoWeb.ChatModels
{
    public class ChatHub : Hub
    {
        private ChatHubData _chatData;
        public ChatHub(ChatHubData data)
        {
            _chatData = data;
        }
        public async Task AddUser(string userName)
        {
            await _chatData.AddNewUserAsync(Context.ConnectionId, userName);
            await Clients.Caller.SendAsync("ConfirmUser", userName);
        }
        public async Task SendMessage(string message)
        {
            var user = _chatData.GetUser(Context.ConnectionId);
            await Clients.All.SendAsync("ReceiveMessage", user.Name, message);
        }
        public async Task CreateGroup(string groupName)
        {
            _chatData.AddGroup(groupName);
            await Clients.Caller.SendAsync("ConfirmGroup", groupName);
        }

        public async Task JoinGroup(string groupName)
        {
            var user = _chatData.GetUser(Context.ConnectionId);
            var group = _chatData.GetGroup(groupName);
            group.AddUser(user);
            await Clients.Group(groupName).SendAsync("NewJoin", user.Name);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.SendAsync("ConfirmJoin", groupName);
        }
        public async Task SendMessageToMyGroup(string message)
        {
            var user = _chatData.GetUser(Context.ConnectionId);
            var group = _chatData.GetGroup(user);
            await Clients.Group(_chatData.GetGroup(user).Name).SendAsync("ReceiveGroupMessage", user.Name, group.Name, message);
        }

    }
}
