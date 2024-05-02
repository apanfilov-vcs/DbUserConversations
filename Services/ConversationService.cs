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

        public async Task<ServiceResponse<string>> AddConversation(string userId)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                
                if (dbUser is null)
                {
                    throw new Exception($"User with id '{userId}' not found.");
                }

                var dbConversation = new Conversation("New Conversation");

                _dbContext.Conversations.Add(dbConversation);
                dbConversation.Users.Add(dbUser);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = dbConversation.Id;
                serviceResponse.Message = $"Successfully added new conversation to database";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Conversation>> DeleteConversationById(string id)
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

        public async Task<ServiceResponse<Conversation>> UpdateConversationNameById(string id, string name)
        {
            var serviceResponse = new ServiceResponse<Conversation>();

            try
            {
                var dbConversation = await _dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == id);

                if (dbConversation is null)
                {
                    throw new Exception($"Conversation with id '{id}' not found.");
                }

                dbConversation.Name = name;

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = dbConversation;
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