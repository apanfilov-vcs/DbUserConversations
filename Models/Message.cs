using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DbUserConversations.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public Conversation ToConversation { get; set; }
        public User FromUser { get; set; }
        public string Contents { get; set; } = string.Empty;
        public DateTime TimeSent { get; set; }

        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Message() 
        { 
            TimeSent = DateTime.Now;
        }
        public Message(Conversation toConversation, User fromUser, string contents)
        {
            ToConversation = toConversation;
            FromUser = fromUser;
            Contents = contents;
            TimeSent = DateTime.Now;
        }
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}