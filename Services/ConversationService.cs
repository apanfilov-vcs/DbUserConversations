using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DbUserConversations.Data;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public class ConversationService : IConversationService
    {
        private readonly ApplicationDbContext _dbContext;
        public ConversationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse<Conversation>> AddConversation(HashSet<string> userIds)
        {
            var serviceResponse = new ServiceResponse<Conversation>();

            try
            {
                var dbUsers = new HashSet<User>();

                foreach (string userId in userIds)
                {
                    var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                    if (dbUser is null)
                    {
                        throw new Exception($"User with id '{userId}' not found.");
                    }

                    dbUsers.Add(dbUser);
                }

                var dbConversation = new Conversation("New Conversation", dbUsers);

                _dbContext.Conversations.Add(dbConversation);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = dbConversation;
                serviceResponse.Message = $"Successfully added new conversation to database";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Conversation>> GetConversationById(string id)
        {
            var serviceResponse = new ServiceResponse<Conversation>();

            try
            {
                var dbConversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == id);

                if (dbConversation is null)
                {
                    throw new Exception($"Conversation with id '{id}' not found.");
                }

                serviceResponse.Data = dbConversation;
                serviceResponse.Message = "Successfully fetched conversation from database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Conversation>>> GetConversations()
        {
            var serviceResponse = new ServiceResponse<List<Conversation>>();

            try
            {
                var dbConversations = await _dbContext.Conversations.ToListAsync();

                if (dbConversations is null)
                {
                    throw new Exception("Unable to fetch conversations from database.");
                }

                serviceResponse.Data = dbConversations;
                serviceResponse.Message = "Successfully fetched conversations from database.";
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