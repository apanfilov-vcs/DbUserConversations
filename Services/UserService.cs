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
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetUserDto>> DeleteCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            try
            {
                var claimId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var claimUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == claimId);

                if (claimUser is null)
                {
                    throw new Exception($"User with id '{claimId}' not found.");
                }

                serviceResponse.Data = _mapper.Map<GetUserDto>(claimUser);
                serviceResponse.Message = $"Deleting user with id '{claimId}'.";

                _dbContext.Users.Remove(claimUser);

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

        public async Task<ServiceResponse<GetUserDto>> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            try
            {
                var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var dbUser = await _dbContext.Users
                    .Include(u => u.Conversations)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (dbUser is null)
                {
                    throw new Exception($"User with id '{userId}' not found.");
                }

                serviceResponse.Data = _mapper.Map<GetUserDto>(dbUser);
                serviceResponse.Message = "Successfully fetched logged in user from database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(string id)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            try
            {
                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (dbUser is null)
                {
                    throw new Exception($"User with id '{id}' not found.");
                }

                serviceResponse.Data = _mapper.Map<GetUserDto>(dbUser);
                serviceResponse.Message = "Successfully fetched user from database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> GetUsers()
        {
            var serviceResponse = new ServiceResponse<List<GetUserDto>>();

            try
            {
                var dbUsers = await _dbContext.Users
                    .ToListAsync();

                if (dbUsers is null)
                {
                    throw new Exception("Unable to fetch users from database.");
                }

                serviceResponse.Data = dbUsers.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
                serviceResponse.Message = "Successfully fetched users from database.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateCurrentUserName(ClaimsPrincipal claimsPrincipal, string name)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            try
            {
                var claimId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
                var claimUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == claimId);

                if (claimUser is null)
                {
                    throw new Exception($"User with id '{claimId}' not found.");
                }

                claimUser.Name = name;

                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetUserDto>(claimUser);
                serviceResponse.Message = $"Changed name of user with id '{claimId}'.";
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