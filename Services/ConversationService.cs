using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DbUserConversations.Data;
using DbUserConversations.Models;
using DbUserConversations.DTOs;
using AutoMapper;
using System.Security.Claims;

namespace DbUserConversations.Services
{
    public class ConversationService : IConversationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ConversationService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetConversationDto>> AddConversation(ClaimsPrincipal claimsPrincipal)
        {
            var serviceResponse = new ServiceResponse<GetConversationDto>();

            try
            {
                var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                
                if (dbUser is null)
                {
                    throw new Exception($"User with id '{userId}' not found.");
                }

                var dbConversation = new Conversation("New Conversation");

                _dbContext.Conversations.Add(dbConversation);
                dbConversation.Users.Add(dbUser);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetConversationDto>(dbConversation);
                serviceResponse.Message = $"Successfully added new conversation to database";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetConversationDto>> AddUserToConversationById(ClaimsPrincipal claimsPrincipal, string conversationId, string userId)
        {
            var serviceResponse = new ServiceResponse<GetConversationDto>();

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
                    throw new Exception($"User with id '{claimId}' is not part of conversation with id '{dbConversation.Id}'.");
                }

                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (dbUser is null)
                {
                    throw new Exception($"User with id '{userId}' not found.");
                }

                if (dbConversation.Users.Contains(dbUser))
                {
                    throw new Exception($"Conversation with id '{conversationId}' already contains user with id '{userId}'.");
                }

                dbConversation.Users.Add(dbUser);
                dbUser.Conversations.Add(dbConversation);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetConversationDto>(dbConversation);
                serviceResponse.Message = $"Successfully added user with id '{userId}' to conversation with id '{conversationId}'.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetConversationDto>> DeleteConversationById(ClaimsPrincipal claimsPrincipal, string id)
        {
            var serviceResponse = new ServiceResponse<GetConversationDto>();

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
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (dbConversation is null)
                {
                    throw new Exception($"Conversation with id '{id}' not found.");
                }

                if (dbConversation.Users.Contains(claimUser) is false)
                {
                    throw new Exception($"Conversation with id '{dbConversation.Id}' does not contain user with id '{claimId}'.");
                }

                serviceResponse.Data = _mapper.Map<GetConversationDto>(dbConversation);
                serviceResponse.Message = $"Found conversation with id '{id}'.";

                _dbContext.Conversations.Remove(dbConversation);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Message += " Conversation has been deleted.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetConversationDto>> GetConversationById(ClaimsPrincipal claimsPrincipal, string id)
        {
            var serviceResponse = new ServiceResponse<GetConversationDto>();

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
                    .Include(c => c.ReceivedMessages)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (dbConversation is null)
                {
                    throw new Exception($"Conversation with id '{id}' not found.");
                }

                if (dbConversation.Users.Contains(claimUser) is false)
                {
                    throw new Exception($"Conversation with id '{id}' does not contain user with id '{claimId}'.");
                }

                serviceResponse.Data = _mapper.Map<GetConversationDto>(dbConversation);
                serviceResponse.Message = "Successfully fetched conversation from database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetConversationDto>>> GetConversations(ClaimsPrincipal claimsPrincipal)
        {
            var serviceResponse = new ServiceResponse<List<GetConversationDto>>();

            try
            {
                var claimId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var claimUser = await _dbContext.Users
                    .Include(u => u.Conversations)
                    .ThenInclude(c => c.Users)
                    .FirstOrDefaultAsync(u => u.Id == claimId);

                if (claimUser is null)
                {
                    throw new Exception($"User with id '{claimId}' not found.");
                }

                var dbConversations = claimUser.Conversations.ToList();

                if (dbConversations is null)
                {
                    throw new Exception("Unable to fetch conversations from database.");
                }

                serviceResponse.Data = dbConversations.Select(c => _mapper.Map<GetConversationDto>(c)).ToList();
                serviceResponse.Message = "Successfully fetched conversations from database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetConversationDto>> RemoveUserFromConversationById(ClaimsPrincipal claimsPrincipal, string conversationId)
        {
            var serviceResponse = new ServiceResponse<GetConversationDto>();

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

                dbConversation.Users.Remove(claimUser);

                if (dbConversation.Users.Count == 0)
                {
                    _dbContext.Conversations.Remove(dbConversation);
                }

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetConversationDto>(dbConversation);
                serviceResponse.Message = $"Successfully removed user with id '{claimId}' from conversation with id '{conversationId}'.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetConversationDto>> UpdateConversationNameById(ClaimsPrincipal claimsPrincipal, string id, string name)
        {
            var serviceResponse = new ServiceResponse<GetConversationDto>();

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
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (dbConversation is null)
                {
                    throw new Exception($"Conversation with id '{id}' not found.");
                }

                if (dbConversation.Users.Contains(claimUser) is false)
                {
                    throw new Exception($"Conversation with id '{id}' does not contain user with id '{claimId}'."); 
                }

                dbConversation.Name = name;

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetConversationDto>(dbConversation);
                serviceResponse.Message = $"Changed name of conversation with id '{id}'.";
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