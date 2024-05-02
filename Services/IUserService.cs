using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<List<User>>> GetUsers();
        Task<ServiceResponse<User>> GetUserById(string id);
        Task<ServiceResponse<string>> AddUser(string name);
        Task<ServiceResponse<User>> UpdateUserNameById(string id, string name);
        Task<ServiceResponse<User>> DeleteUserById(string id);
    }
}