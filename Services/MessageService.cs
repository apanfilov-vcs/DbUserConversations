using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DbUserConversations.Data;
using DbUserConversations.DTOs;
using DbUserConversations.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUserConversations.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public MessageService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetMessageDto>> DeleteMessageById(ClaimsPrincipal claimsPrincipal, string messageId)
        {
            var serviceResponse = new ServiceResponse<GetMessageDto>();

            try
            {
                var claimId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var claimUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == claimId);

                if (claimUser is null)
                {
                    throw new Exception($"User with id '{claimId}' not found.");
                }

                var dbMessage = await _dbContext.Messages
                    .Include(m => m.ToConversation)
                    .FirstOrDefaultAsync(m => m.Id == messageId);

                if (dbMessage is null)
                {
                    throw new Exception($"Message with id '{messageId}' not found.");
                }

                if (dbMessage.FromUser != claimUser)
                {
                    throw new Exception($"User with id '{claimId}' does not have message with id '{messageId}'.");
                }

                serviceResponse.Data = _mapper.Map<GetMessageDto>(dbMessage);
                serviceResponse.Message = $"Deleting message with id '{messageId}'.";

                _dbContext.Messages.Remove(dbMessage);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Message += " Message successfully deleted.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetMessageDto>> SendMessage(ClaimsPrincipal claimsPrincipal, string conversationId, string message)
        {
            var serviceResponse = new ServiceResponse<GetMessageDto>();

            try
            {
                var claimId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var claimUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == claimId);

                if (claimUser is null)
                {
                    throw new Exception($"User with id '{claimId}' not found.");
                }

                var dbConversation = await _dbContext.Conversations
                    .Include(c => c.Users)
                    .FirstOrDefaultAsync(c => c.Id == conversationId);

                if (dbConversation is null)
                {
                    throw new Exception($"Conversation with id '{conversationId}' not found.");
                }

                if (dbConversation.Users.Contains(claimUser) is false)
                {
                    throw new Exception($"Conversation with id '{conversationId}' does not contain user with id '{claimId}'.");
                }

                var dbMessage = new Message(dbConversation, claimUser, message);

                dbMessage.ToConversation = dbConversation;
                dbMessage.FromUser = claimUser;
                
                dbConversation.ReceivedMessages.Add(dbMessage);
                claimUser.SentMessages.Add(dbMessage);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetMessageDto>(dbMessage);
                serviceResponse.Message = $"Successfully added message with id '{dbMessage.Id}' to conversation with id '{conversationId}'.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetMessageDto>> UpdateMessageById(ClaimsPrincipal claimsPrincipal, string messageId, string message)
        {
            var serviceResponse = new ServiceResponse<GetMessageDto>();

            try
            {
                var claimId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var claimUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == claimId);

                if (claimUser is null)
                {
                    throw new Exception($"User with id '{claimId}' not found.");
                }

                // var dbMessages = await _dbContext.Messages.ToListAsync();
                var dbMessage = await _dbContext.Messages
                    .Include(m => m.ToConversation)
                    .FirstOrDefaultAsync(m => m.Id == messageId);

                if (dbMessage is null)
                {
                    throw new Exception($"Message with id '{messageId}' not found.");
                }

                if (dbMessage.FromUser.Id != claimId)
                {
                    throw new Exception($"Message with id '{messageId}' was not sent by user with id '{claimId}'.");
                }

                dbMessage.Contents = message;

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetMessageDto>(dbMessage);
                serviceResponse.Message = $"Successfully updated message with id '{messageId}'.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}