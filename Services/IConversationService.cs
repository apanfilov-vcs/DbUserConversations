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
        Task<ServiceResponse<Conversation>> AddConversation(HashSet<string> userIds);
    }
}