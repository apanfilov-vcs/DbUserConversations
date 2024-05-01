using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DbUserConversations.Data;
using DbUserConversations.Models;

namespace DbUserConversations.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        public UserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse<User>> AddUser(string name)
        {
            var serviceResponse = new ServiceResponse<User>();

            try
            {
                var user = new User(name);
                
                await _dbContext.Users.AddAsync(user);

                serviceResponse.Data = user;
                serviceResponse.Message = $"Successfully added user '{name}' to database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> GetUserById(string id)
        {
            var serviceResponse = new ServiceResponse<User>();

            try
            {
                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (dbUser is null)
                {
                    throw new Exception($"User with id '{id}' not found.");
                }

                serviceResponse.Data = dbUser;
                serviceResponse.Message = "Successfully fetched user from database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<User>>> GetUsers()
        {
            var serviceResponse = new ServiceResponse<List<User>>();

            try
            {
                var dbUsers = await _dbContext.Users.ToListAsync();

                if (dbUsers is null)
                {
                    throw new Exception("Unable to fetch users from database.");
                }

                serviceResponse.Data = dbUsers;
                serviceResponse.Message = "Successfully fetched users from database.";
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