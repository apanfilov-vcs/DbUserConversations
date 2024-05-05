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
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetConversationDto>>>> GetConversations()
        {
            var serviceRequest = await _conversationService.GetConversations(User);

            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetConversationDto>>> GetConversationById(string id)
        {
            var serviceRequest = await _conversationService.GetConversationById(User, id);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpPost("AddConversation")]
        public async Task<ActionResult<ServiceResponse<GetConversationDto>>> AddConversation()
        {
            var serviceRequest = await _conversationService.AddConversation(User);
            
            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return serviceRequest;
        }

        [HttpPost("AddUserToConversationById")]
        public async Task<ActionResult<ServiceResponse<GetConversationDto>>> AddUserToConversationById(string conversationId, string userId)
        {
            var serviceRequest = await _conversationService.AddUserToConversationById(User, conversationId, userId);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpPut("UpdateConversationById")]
        public async Task<ActionResult<ServiceResponse<GetConversationDto>>> UpdateConverationNameById(string id, string name)
        {
            var serviceRequest = await _conversationService.UpdateConversationNameById(User, id, name);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpDelete("RemoveUserFromConversationById")]
        public async Task<ActionResult<ServiceResponse<GetConversationDto>>> RemoveUserFromConversationById(string conversationId)
        {
            var serviceRequest = await _conversationService.RemoveUserFromConversationById(User, conversationId);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        // Due to new security implementations, the DeleteConversationById action on ConversationController has been disables.
        // Only enable it on an admin features controller.

        // [HttpDelete("DeleteConversationById")]
        // public async Task<ActionResult<ServiceResponse<GetConversationDto>>> DeleteConversationById(string id)
        // {
        //     var serviceRequest = await _conversationService.DeleteConversationById(id);

        //     if (serviceRequest.Success is false)
        //     {
        //         return NotFound(serviceRequest);
        //     }

        //     return Ok(serviceRequest);
        // }
    }
}