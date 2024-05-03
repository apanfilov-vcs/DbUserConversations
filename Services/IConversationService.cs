using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.DTOs;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public interface IConversationService
    {
        Task<ServiceResponse<List<GetConversationDto>>> GetConversations();
        Task<ServiceResponse<GetConversationDto>> GetConversationById(string id);
        Task<ServiceResponse<GetConversationDto>> AddConversation(string userId);
        Task<ServiceResponse<GetConversationDto>> AddUserToConversationById(string conversationId, string userId);
        Task<ServiceResponse<GetConversationDto>> UpdateConversationNameById(string id, string name);
        Task<ServiceResponse<GetConversationDto>> RemoveUserFromConversationById(string conversationId, string userId);
        Task<ServiceResponse<GetConversationDto>> DeleteConversationById(string id);
    }
}