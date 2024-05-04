using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbUserConversations.Models
{
    public class AuthenticationJWT
    {
        public string Value { get; set; } = string.Empty;   

        public AuthenticationJWT() { }
        public AuthenticationJWT(string value)
        {
            Value = value;
        }
    }
}