using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.DataBase
{
    public static class ExistingUsers
    {
        public static List<User> RegisteredUsers = new List<User>() 
        { 
            new User()
            {
                username = "Gazza"            
            },

            new User()
            {
                username = "Mario"
            }
        };

        public static Dictionary<string, string> UserPasswords = new Dictionary<string, string>() { { "Gazza", "psw" }, { "Mario", "supermario" } };

    }
}
