using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        // for hasing the password
        public byte[] PasswordHash { get; set; }
        // to get unique password
        public byte[] PasswordSalt { get; set; } 
        public List<Character>? Characters { get; set; }
    }
}