using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public interface IConversationService
    {
        Task<ServiceResponse<List<Conversation>>> GetConversations();
        Task<ServiceResponse<Conversation>> GetConversationById(string id);
        Task<ServiceResponse<string>> AddConversation(string userId);
        Task<ServiceResponse<Conversation>> UpdateConversationNameById(string id, string name);
        Task<ServiceResponse<Conversation>> DeleteConversationById(string id);
    }
}