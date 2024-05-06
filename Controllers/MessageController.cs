using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.DTOs;
using DbUserConversations.Models;
using DbUserConversations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DbUserConversations.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("SendMessage")]
        public async Task<ActionResult<ServiceResponse<GetMessageDto>>> SendMessage(string conversationId, string message)
        {
            var serviceRequest = await _messageService.SendMessage(User, conversationId, message);

            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpPut("UpdateMessageById")]
        public async Task<ActionResult<ServiceResponse<GetMessageDto>>> UpdateMessageById(string messageId, string message)
        {
            var serviceRequest = await _messageService.UpdateMessageById(User, messageId, message);

            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpDelete("DeleteMessageById")]
        public async Task<ActionResult<ServiceResponse<GetMessageDto>>> DeleteMessageById(string messageId)
        {
            var serviceRequest = await _messageService.DeleteMessageById(User, messageId);

            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return Ok(serviceRequest);
        }
    }
}