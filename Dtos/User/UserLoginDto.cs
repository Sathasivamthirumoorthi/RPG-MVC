using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Dtos.User
{
    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        // for hasing the password
        public string Password {get; set;} = string.Empty;
    }
}