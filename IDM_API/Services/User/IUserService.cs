using IDM_API.Data;
using IDM_API.Data.MapperData;
using IDM_API.Data.UserData;
using IDM_API.Entities;

namespace IDM_API.Services.User
{
    public interface IUserService
	{
		Task<ApiResponse<LoginResult>> Login(LoginRequest request);
		Task<ApiResponse<UserCreateDTO>> Register(UserCreateDTO newUser);
		Task<ApiResponse<string>> ChangePassword(Guid UserID, ChangePasswordRequest request);
		Task<bool> IsExist(string userName);
		Task<ApiResponse<Object>> Logout(string token);
		Task<ApiResponse<UserProfile>> GetProfile(Guid UserID);
		Task<ApiResponse<List<UserProfile>>> GetUsers();
		Task<ApiResponse<UserUpdateDTO>> UpdateUser(Guid UserID, UserUpdateDTO user);
		Task<ApiResponse<UserProfile>> SetStatus(Guid UserID, Guid Author);
		Task<ApiResponse<Object>> DeleteUser(Guid UserID);
		ApiResponse<Object> SendBackupEmail(string recipientEmail, string token);
	}
}
