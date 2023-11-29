using AutoMapper;
using BCrypt.Net;
using IDM_API.Data;
using IDM_API.Data.MapperData;
using IDM_API.Data.UserData;
using IDM_API.Entities;
using IDM_API.Services.Jwt;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace IDM_API.Services.User
{
    public class UserService : IUserService
	{
		private readonly IDMContext _context;
		private readonly IJwtService _jwtService;
		private readonly IMapper _mapper;

		public UserService(IDMContext context, IJwtService jwtService, IMapper mapper)
        {
			_context = context;
			_jwtService = jwtService;
			_mapper = mapper;
		}

		public async Task<ApiResponse<LoginResult>> Login(LoginRequest request)
		{
			try
			{
				await Task.CompletedTask;
				var user = _context.tbl_users.FirstOrDefault(x => x.UsernameOrEmail == request.UsernameOrEmail);

				if (user == null)
				{
					return new ApiResponse<LoginResult>
					{
						Success = false,
						Message = "Người dùng không tồn tại.",
						Status = (int)HttpStatusCode.Unauthorized
					};
				}

				if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
				{
					return new ApiResponse<LoginResult>
					{
						Success = false,
						Message = "Sai mật khẩu, nhập lại mật khẩu.",
						Status = (int)HttpStatusCode.Unauthorized
					};
				}

				var roles = (from u in _context.tbl_users
							join ur in _context.tbl_user_roles
							on u.UserID equals ur.UserID
							join r in _context.tbl_roles
							on ur.RoleID equals r.RoleID
							where u.UserID == user.UserID
							select r.RoleName).ToArray();

				var claims = new List<Claim>
				{
					new Claim("UserID", user.UserID.ToString()),
					new Claim(ClaimTypes.NameIdentifier, user.Fullname),
				};

				foreach(var role in roles)
				{
					claims.Add(new Claim(ClaimTypes.Role, role));
				}

				var result = _jwtService.GenerateToken(user, claims, DateTime.Now);
				result.UserResult = new UserResult
				{
					UserID = user.UserID,
					UserNameOrEmail = user.UsernameOrEmail,
					Roles = roles,
				};

				return new ApiResponse<LoginResult>
				{
					Success = true,
					Message = "Đăng nhập thành công.",
					Data = result,
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<LoginResult>
				{
					Success = false,
					Message = "User service - Login: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<UserCreateDTO>> Register(UserCreateDTO newUser)
		{
			try
			{
				if(newUser.UsernameOrEmail == null || newUser.PasswordHash == null) 
				{
					return new ApiResponse<UserCreateDTO>
					{
						Success = false,
						Message = "Tên đăng nhập hoặc mật khẩu không hợp lệ.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				if(await IsExist(newUser.UsernameOrEmail))
				{
					return new ApiResponse<UserCreateDTO>
					{
						Success = false,
						Message = "Username hoặc email này đã tồn tại.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var userEntity = _mapper.Map<tbl_user>(newUser);

				await _context.tbl_users.AddAsync(userEntity);

				var normalRole = await _context.tbl_roles.FirstOrDefaultAsync(x => x.RoleCode == 300);
				if (normalRole == null)
				{
					return new ApiResponse<UserCreateDTO>
					{
						Success = false,
						Message = "Không tồn tại quyền để tạo tài khoản mới.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				await _context.SaveChangesAsync();

				await RelateRole(normalRole.RoleID, userEntity.UserID);

				return new ApiResponse<UserCreateDTO>
				{
					Success = true,
					Message = "Tạo người dùng mới thành công.",
					Data = newUser,
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<UserCreateDTO>
				{
					Success = false,
					Message = "User service - Register: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		private async Task RelateRole(int roleID, Guid userID)
		{
			var userRole = new tbl_user_role
			{
				RoleID = roleID,
				UserID = userID,
			};

			await _context.tbl_user_roles.AddAsync(userRole);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> IsExist(string userName)
		{
			try
			{
				await Task.CompletedTask;
				bool result = await _context.tbl_users.FirstOrDefaultAsync(u => u.UsernameOrEmail == userName) is not null;
				return result;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<ApiResponse<string>> ChangePassword(Guid UserID, ChangePasswordRequest request)
		{
			try
			{
				await Task.CompletedTask;
				var user = await _context.tbl_users.FindAsync(UserID);
				if (user == null)
				{
					return new ApiResponse<string>
					{
						Success = false,
						Message = "Người dùng không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}
				if(!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
				{
					return new ApiResponse<string>
					{
						Success = false,
						Message = "Mật khẩu cũ không đúng.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
				_context.tbl_users.Entry(user).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return new ApiResponse<string>
				{
					Success = true,
					Message = "Đổi mật khẩu thành công, vui lòng đăng nhập lại.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<string>
				{
					Success = false,
					Message = "User service - ChangePassword: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<Object>> Logout(string token)
		{
			try
			{
				await Task.CompletedTask;
				bool result = await _jwtService.RevokeToken(token);
				if(!result)
				{
					return new ApiResponse<Object>
					{
						Success = false,
						Message = "Token không hợp lệ.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				else
					return new ApiResponse<Object>
					{
						Success = true,
						Message = "Đăng xuất thành công.",
						Status = (int)HttpStatusCode.OK
					};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "User service - Logout: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<UserProfile>> GetProfile(Guid UserID)
		{
			try
			{
				await Task.CompletedTask;
				var user = await _context.tbl_users.FindAsync(UserID);
				if(user == null)
				{
					return new ApiResponse<UserProfile>
					{
						Success = false,
						Message = "Người dùng không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<UserProfile>
				{
					Success = true,
					Message = "Lấy người dùng thành công.",
					Data = _mapper.Map<UserProfile>(user),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<UserProfile>
				{
					Success = false,
					Message = "UserService - GetProfile: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<UserProfile>>> GetUsers()
		{
			try
			{
				await Task.CompletedTask;
				var users = await _context.tbl_users.ToListAsync();

				if(users == null || users.Count() <= 0)
				{
					return new ApiResponse<List<UserProfile>>
					{
						Success = false,
						Message = "Danh sách trống.",
						Status = (int)HttpStatusCode.OK
					};
				}

				return new ApiResponse<List<UserProfile>>
				{
					Success = true,
					Message = "Lấy danh sách người dùng thành công.",
					Data = users.Select(x => _mapper.Map<UserProfile>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<UserProfile>>
				{
					Success = false,
					Message = "UserService - GetUsers: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<UserUpdateDTO>> UpdateUser(Guid UserID, UserUpdateDTO user)
		{
			try
			{
				await Task.CompletedTask;

				if(UserID != user.UserID)
				{
					return new ApiResponse<UserUpdateDTO>
					{
						Success = false,
						Message = "Người dùng không đúng.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var userInData = await GetByID(UserID);
				if(userInData == null)
				{
					return new ApiResponse<UserUpdateDTO>
					{
						Success = false,
						Message = "Người dùng không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				_mapper.Map(user, userInData);
				

				_context.tbl_users.Update(userInData);
				await _context.SaveChangesAsync();

				return new ApiResponse<UserUpdateDTO>
				{
					Success = true,
					Message = "Cập nhật người dùng thành công.",
					Data = user,
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<UserUpdateDTO>
				{
					Success = false,
					Message = "UserService - UpdateUser: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		private async Task<tbl_user?> GetByID(Guid UserID)
		{
			try
			{
				await Task.CompletedTask;
				return await _context.tbl_users.FindAsync(UserID);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<ApiResponse<UserProfile>> SetStatus(Guid UserID, Guid Author)
		{
			try
			{
				await Task.CompletedTask;
				var userInData = await GetByID(UserID);
				if(userInData == null)
				{
					return new ApiResponse<UserProfile>
					{
						Success = false,
						Message = "Người dùng không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				if (Author != null)
				{
					userInData.UpdatedBy = Author;
				}

				userInData.UpdatedDateTime = DateTime.Now;

				if (userInData.Status == 0)
					userInData.Status = 1;
				else
					userInData.Status = 0;

				_context.Entry(userInData).State = EntityState.Modified;
				await _context.SaveChangesAsync();
				return new ApiResponse<UserProfile>
				{
					Success = true,
					Message = "Thay đổi trạng thái người dùng thành công.",
					Status = (int)HttpStatusCode.OK,
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<UserProfile>
				{
					Success = false,
					Message = "UserService - SetStatus: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> DeleteUser(Guid UserID)
		{
			try
			{
				await Task.CompletedTask;
				await _context.Database.ExecuteSqlInterpolatedAsync($"sp_deleteUser {UserID}");

				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Xóa người dùng thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "UserService - DeleteUser: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public ApiResponse<Object> SendBackupEmail(string recipientEmail, string token)
		{
			string senderEmail = "minh.quang1720@gmail.com";
			string senderPassword = "acanhcamlay2";

			SmtpClient smtpClient = new SmtpClient("smtp.test.com");
			smtpClient.Port = 587;
			smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
			smtpClient.EnableSsl = true;

			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress(senderEmail);
			mailMessage.To.Add(recipientEmail);
			mailMessage.Subject = "Xác thực yêu cầu sao lưu tài khoản";
			mailMessage.Body = $"Vui lòng nhấp vào liên kết sau để xác thực yêu cầu sao lưu tài khoản: {token}";
			mailMessage.IsBodyHtml = false;

			try
			{
				smtpClient.Send(mailMessage);
				return new ApiResponse<Object>
				{
					Success = true,
					Message = "Gửi mail thành công",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "UserService - SendBackupRequest: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
