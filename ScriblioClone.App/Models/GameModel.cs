using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriblioClone.App.Models
{
    public class GameModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string LobbyLeaderName { get; set; }
        
 
    }
    public class UserModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
    }
}
