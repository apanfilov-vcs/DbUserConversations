using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DbUserConversations.DTOs;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<List<GetUserDto>>> GetUsers();
        Task<ServiceResponse<GetUserDto>> GetUserById(string id);
        Task<ServiceResponse<GetUserDto>> GetLoggedInUser(ClaimsPrincipal claimsPrincipal);
        Task<ServiceResponse<GetUserDto>> AddUser(string name);
        Task<ServiceResponse<GetUserDto>> UpdateUserNameById(ClaimsPrincipal claimsPrincipal, string id, string name);
        Task<ServiceResponse<GetUserDto>> DeleteUserById(ClaimsPrincipal claimsPrincipal, string id);
    }
}