using System.Collections.Generic;

namespace LudoWeb.ChatModels
{
    public class Group
    {
        public Group()
        {
            Users = new();
        }
        public string Name { get; set; }
        private List<User> Users { get; set; }
        public bool UserExists(User user) => Users.Exists(x => x == user);
        public void AddUser(User user) => Users.Add(user);
    }
}