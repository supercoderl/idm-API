using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.MapperData;
using IDM_API.Entities;
using IDM_API.Services.Jwt;
using IDM_API.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AuthController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IJwtService _jwtService;
		private readonly IMapper _mapper;

		public AuthController(IUserService userService, IJwtService jwtService, IMapper mapper)
        {
			_userService = userService;
			_jwtService = jwtService;
			_mapper = mapper;
		}

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
			var result = await _userService.Login(request);
			return StatusCode(result.Status, result);
        }

		[AllowAnonymous]
		[HttpPost("register")]
		public async Task<IActionResult> Register(UserCreateDTO newUser)
		{
			newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.PasswordHash);
			if(User.Identity.IsAuthenticated)
			{
				newUser.CreatedBy = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserID")!.Value);
			}
			var result = await _userService.Register(newUser);
			return StatusCode(result.Status, result);
		}

		[HttpPut("change-password")]
		public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
		{
			Guid userID = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "UserID")!.Value);
			var result = await _userService.ChangePassword(userID, request);
			return StatusCode(result.Status, result);
		}

		[HttpPost("logout")]
		public async Task<IActionResult> RevokeToken(RefreshTokenRequest refreshToken)
		{
			var result = await _userService.Logout(refreshToken.RefreshToken);
			return StatusCode(result.Status, result);
		}

		[HttpPost("refresh-token")]
		public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
		{
			var result = await _jwtService.Refresh(request.RefreshToken, DateTime.Now);
			return StatusCode(result.Status, result);
		}
    }
}
