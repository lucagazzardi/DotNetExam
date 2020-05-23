using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class User
    {
        public string username { get; set; }

        public String token { get; set; }

        public string connectionId { get; set; }

        public int jolly { get; set; }
    }

    public class UserInfo
    {
        public string username { get; set; }

        public string password { get; set; }
    }
}
