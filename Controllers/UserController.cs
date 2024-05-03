using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbUserConversations.DTOs;
using DbUserConversations.Models;
using DbUserConversations.Services;
using Microsoft.AspNetCore.Mvc;

namespace DbUserConversations.Controllers
{
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

        [HttpPost("AddUser")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> AddUser(string name)
        {
            var serviceRequest = await _userService.AddUser(name);

            if (serviceRequest.Success is false)
            {
                return BadRequest(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpPut("UpdateUserNameById")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateUserNameById(string id, string name)
        {
            var serviceRequest = await _userService.UpdateUserNameById(id, name);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }

        [HttpDelete("DeleteUserById")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> DeleteUserById(string id)
        {
            var serviceRequest = await _userService.DeleteUserById(id);

            if (serviceRequest.Success is false)
            {
                return NotFound(serviceRequest);
            }

            return Ok(serviceRequest);
        }
    }
}