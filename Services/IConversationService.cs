using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DbUserConversations.DTOs;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public interface IConversationService
    {
        Task<ServiceResponse<List<GetConversationDto>>> GetConversations(ClaimsPrincipal claimsPrincipal);
        Task<ServiceResponse<GetConversationDto>> GetConversationWithMessagesById(ClaimsPrincipal claimsPrincipal, string id);
        Task<ServiceResponse<GetConversationDto>> AddConversation(ClaimsPrincipal claimsPrincipal, string name);
        Task<ServiceResponse<GetConversationDto>> AddUserToConversationById(ClaimsPrincipal claimsPrincipal, string conversationId, string userId);
        Task<ServiceResponse<GetConversationDto>> UpdateConversationNameById(ClaimsPrincipal claimsPrincipal, string id, string name);
        Task<ServiceResponse<GetConversationDto>> RemoveUserFromConversationById(ClaimsPrincipal claimsPrincipal, string conversationId);
        Task<ServiceResponse<GetConversationDto>> DeleteConversationById(ClaimsPrincipal claimsPrincipal, string id);
    }
}