using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbUserConversations.Services
{
    public interface IPasswordService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}