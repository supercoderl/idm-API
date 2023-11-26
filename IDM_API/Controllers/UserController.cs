using AutoMapper;
using Azure.Core;
using IDM_API.Data.UserData;
using IDM_API.Entities;
using IDM_API.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public UserController(IUserService userService, IMapper mapper)
        {
			_userService = userService;
			_mapper = mapper;
		}

		[HttpGet("profile")]
		[Authorize]
		public async Task<IActionResult> GetProfile()
		{
			var userAuthID = Guid.Parse(User.FindFirstValue("UserID")!);
			var result = await _userService.GetProfile(userAuthID);
			return StatusCode(result.Status, result);
		}

		[HttpGet("users")]
		public async Task<IActionResult> GetUsers()
		{
			var result = await _userService.GetUsers();
			return StatusCode(result.Status, result);
		}

		[HttpPut("update-user/{UserID}")]
		public async Task<IActionResult> UpdateUser(Guid UserID, UserUpdateDTO request)
		{
			var userAuthID = Guid.Parse(User.FindFirstValue("UserID")!);
			if(userAuthID != null)
			{
				request.UpdatedBy = userAuthID;
			}
			request.UpdatedDateTime = DateTime.Now;
			var result = await _userService.UpdateUser(UserID, request);
			return StatusCode(result.Status, result);
		}

		[HttpPut("set-status/{UserID}")]
		public async Task<IActionResult> SetStatus(Guid UserID)
		{
			var userAuthID = Guid.Parse(User.FindFirstValue("UserID")!);
			var result = await _userService.SetStatus(UserID, userAuthID);
			return StatusCode(result.Status, result);
		}

		[HttpDelete("delete-user/{UserID}")]
		public async Task<IActionResult> DeleteUser(Guid UserID)
		{
			var result = await _userService.DeleteUser(UserID);
			return StatusCode(result.Status, result);
		}
    }
}
