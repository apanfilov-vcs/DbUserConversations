using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.Models;

namespace DbUserConversations.DTOs
{
    public class GetMessageDto
    {
        public string? Id { get; set; }
        public Conversation? ToConversation { get; set; }
        public User? FromUser { get; set; }
        public string? Contents { get; set; }
        public DateTime? TimeSent { get; set; }
    }
}