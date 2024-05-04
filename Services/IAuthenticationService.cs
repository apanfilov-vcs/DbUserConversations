using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.DTOs;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse<GetUserDto>> Register(string name, string password);
        Task<ServiceResponse<AuthenticationJWT>> Login(string name, string password);
        Task<bool> UserExists(string name);
    }
}