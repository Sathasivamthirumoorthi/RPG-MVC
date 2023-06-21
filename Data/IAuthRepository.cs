using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user , string password);
        Task<ServiceResponse<string>> Login(string userename, string password);
        Task<bool> UserExists(string username);
    }
}