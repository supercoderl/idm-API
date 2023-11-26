using IDM_API.Data;
using IDM_API.Entities;
using System.Security.Claims;

namespace IDM_API.Services.Jwt
{
	public interface IJwtService
	{
		LoginResult GenerateToken(tbl_user user, List<Claim> claims, DateTime now);
		Task RemoveRefreshToken(string username);
		Task<ApiResponse<LoginResult>> Refresh(string refreshToken, DateTime now);
		Task<bool> RevokeToken(string token);
	}
}
