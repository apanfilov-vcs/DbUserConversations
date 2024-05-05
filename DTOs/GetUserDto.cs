using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.Models;

namespace DbUserConversations.DTOs
{
    public class GetUserDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<GetUserConversationDto>? Conversations { get; set; }
    }
}