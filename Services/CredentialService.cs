using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DbUserConversations.Data;
using DbUserConversations.Models;
using Microsoft.IdentityModel.Tokens;

namespace DbUserConversations.Services
{
    public class CredentialService : ICredentialService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public CredentialService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            var hmac = new HMACSHA512();

            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public ServiceResponse<AuthenticationJWT> CreateToken(User user)
        {
            var serviceResponse = new ServiceResponse<AuthenticationJWT>();

            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Name)
                };
                var authenticationToken = _configuration.GetSection("Authentication:Token").Value;

                if (authenticationToken is null)
                {
                    throw new Exception("Authentication:Token not found in appsettings.json file.");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationToken));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var serializedToken = tokenHandler.WriteToken(token);

                serviceResponse.Data = new AuthenticationJWT(serializedToken);
                serviceResponse.Message = $"Successfully created and serialized new JWT for user '{user.Name}'.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}