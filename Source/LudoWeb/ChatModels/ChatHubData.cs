using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LudoWeb.ChatModels
{
    public class ChatHubData
    {
        private HashSet<User> Users;
        private HashSet<Group> Groups;
        public ChatHubData()
        {
            Users = new();
            Groups = new();
        }
        public async Task AddNewUserAsync(string connection, string username)
        {
            Users.Add(new User()
            {
                ConnectionId = connection,
                Name = username
            });
        }

        public Group GetGroup(User user) => Groups.FirstOrDefault(x => x.UserExists(user));
        public Group GetGroup(string groupName) => Groups.FirstOrDefault(x => x.Name == groupName);
        public void AddGroup(string name)
        {
            Groups.Add(new Group
            {
                Name = name
            });
        }
        public void AddToGroup(string connectionId, string groupName)
        {
            var user = Users.FirstOrDefault(x => x.ConnectionId == connectionId);
            var group = Groups.FirstOrDefault(x => x.Name == groupName);
            group.AddUser(user);
        }

        public User GetUser(string connectionId) => Users.First(x => x.ConnectionId == connectionId);

    }
}