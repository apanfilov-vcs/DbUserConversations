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

        public async Task<ServiceResponse<string>> AddUser(string name)
        {
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                var user = new User(name);
                
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = user.Id;
                serviceResponse.Message = $"Successfully added user '{name}' to database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> DeleteUserById(string id)
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
                serviceResponse.Message = $"Found user with id '{id}'.";

                _dbContext.Users.Remove(dbUser);

                await _dbContext.SaveChangesAsync();

                serviceResponse.Message += " User has been deleted.";
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

        public async Task<ServiceResponse<User>> UpdateUserNameById(string id, string name)
        {
            var serviceResponse = new ServiceResponse<User>();

            try
            {
                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (dbUser is null)
                {
                    throw new Exception($"User with id '{id}' not found.");
                }

                dbUser.Name = name;

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = dbUser;
                serviceResponse.Message = $"Changed name of user with id '{id}'.";
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