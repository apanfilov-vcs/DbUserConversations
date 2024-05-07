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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> GetUsers()
        {
            var serviceRequest = await _userService.GetUsers();

            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUserById(string id)
        {
            var serviceRequest = await _userService.GetUserById(id);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<ServiceResponse<GetUserConversationDto>>> GetLoggedInUser()
        {
            var serviceRequest = await _userService.GetCurrentUser(User);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpPut("UpdateCurrentUserName")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateUserNameById(string name)
        {
            var serviceRequest = await _userService.UpdateCurrentUserName(User, name);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpDelete("DeleteCurrentUser")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> DeleteUserById()
        {
            var serviceRequest = await _userService.DeleteCurrentUser(User);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }
    }
}