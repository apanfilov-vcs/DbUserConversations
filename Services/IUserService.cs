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
        Task<ServiceResponse<User>> AddUser(string name);
    }
}