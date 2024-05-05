using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DbUserConversations.DTOs;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public interface IMessageService
    {
        Task<ServiceResponse<GetMessageDto>> SendMessage(ClaimsPrincipal claimsPrincipal, string conversationId, string message);
        Task<ServiceResponse<GetMessageDto>> UpdateMessage(ClaimsPrincipal claimsPrincipal, string messageId, string message);
        Task<ServiceResponse<GetMessageDto>> DeleteMessageById(ClaimsPrincipal claimsPrincipal, string messageId);
    }
}