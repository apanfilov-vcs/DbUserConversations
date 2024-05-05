using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.Models;

namespace DbUserConversations.DTOs
{
    public class GetConversationDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public List<GetConversationUserDto>? Users { get; set; }
        public List<GetMessageDto>? Messages { get; set; }
    }
}