using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.Models;
using DbUserConversations.Services;
using Microsoft.AspNetCore.Mvc;

namespace DbUserConversations.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Conversation>>>> GetConversations()
        {
            var serviceRequest = await _conversationService.GetConversations();

            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Conversation>>> GetConversationById(string id)
        {
            var serviceRequest = await _conversationService.GetConversationById(id);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpPost("AddConversation")]
        public async Task<ActionResult<ServiceResponse<string>>> AddConversation(string userId)
        {
            var serviceRequest = await _conversationService.AddConversation(userId);
            
            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return serviceRequest;
        }

        [HttpPut("UpdateConversationById")]
        public async Task<ActionResult<ServiceResponse<Conversation>>> UpdateConverationNameById(string id, string name)
        {
            var serviceRequest = await _conversationService.UpdateConversationNameById(id, name);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpDelete("DeleteConversationById")]
        public async Task<ActionResult<ServiceResponse<Conversation>>> DeleteConversationById(string id)
        {
            var serviceRequest = await _conversationService.DeleteConversationById(id);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }
    }
}