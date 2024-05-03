using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbUserConversations.DTOs
{
    public class GetUserConversationDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}