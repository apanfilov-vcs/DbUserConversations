using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbUserConversations.Data;
using DbUserConversations.DTOs;
using DbUserConversations.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUserConversations.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICredentialService _credentialService;
        public AuthenticationService(ApplicationDbContext dbContext, IMapper mapper, ICredentialService credentialService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _credentialService = credentialService;
        }

        public async Task<ServiceResponse<AuthenticationJWT>> Login(string name, string password)
        {
            var serviceResponse = new ServiceResponse<AuthenticationJWT>();

            try
            {
                var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
                
                if (dbUser is null)
                {
                    throw new Exception($"User with name '{name}' not found.");
                }

                if (_credentialService.VerifyPasswordHash(password, dbUser.PasswordHash, dbUser.PasswordSalt) is false)
                {
                    throw new Exception($"Wrong password entered for user '{name}'.");
                }

                serviceResponse.Data = _credentialService.CreateToken(dbUser).Data;
                serviceResponse.Message = $"Successfully generated JWT token for user '{name}'.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetUserDto>> Register(string name, string password)
        {
            var serviceResponse = new ServiceResponse<GetUserDto>();

            try
            {
                var user = new User(name);

                if (await UserExists(name))
                {
                    throw new Exception($"User with name '{name}' already exists.");
                }

                _credentialService.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _dbContext.Users.Add(user);
                
                await _dbContext.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetUserDto>(user);
                serviceResponse.Message = $"Successfully registered user with id '{user.Id}'";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<bool> UserExists(string name)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());

            if (dbUser is null)
            {
                return false;
            }

            return true;
        }
    }
}