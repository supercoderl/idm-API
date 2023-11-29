using AutoMapper;
using IDM_API.Data.ScheduleData;
using IDM_API.Data;
using IDM_API.Entities;
using System.Net;
using IDM_API.Data.Role;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IDM_API.Services.Role
{
	public class RoleService : IRoleService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;

		public RoleService(IDMContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<ApiResponse<RoleDTO>> CreateRole(CreateRoleDTO newRole)
		{
			try
			{
				await Task.CompletedTask;
				var roleEntity = _mapper.Map<tbl_role>(newRole);
				await _context.tbl_roles.AddAsync(roleEntity);
				await _context.SaveChangesAsync();
				return new ApiResponse<RoleDTO>
				{
					Success = true,
					Message = "Tạo quyền thành công.",
					Data = _mapper.Map<RoleDTO>(roleEntity),
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<RoleDTO>
				{
					Success = false,
					Message = "RoleService - CreateRole: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> DeleteRole(int roleID)
		{
			try
			{
				await Task.CompletedTask;
				var role = await _context.tbl_roles.FindAsync(roleID);
				if (role == null)
					return new ApiResponse<object>
					{
						Success = false,
						Message = "Không tìm thấy quyền.",
						Status = (int)HttpStatusCode.NotFound
					};

				_context.tbl_roles.Remove(role);

				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Xóa quyền thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "RoleService - DeleteRole: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<RoleDTO>> GetRoleByID(int roleID)
		{
			try
			{
				await Task.CompletedTask;
				var role = await _context.tbl_roles.FindAsync(roleID);
				if (role == null)
				{
					return new ApiResponse<RoleDTO>
					{
						Success = false,
						Message = "Không có quyền.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<RoleDTO>
				{
					Success = true,
					Message = "Tìm thấy quyền.",
					Data = _mapper.Map<RoleDTO>(role),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<RoleDTO>
				{
					Success = false,
					Message = "RoleService - GetRoleByID: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<RoleDTO>>> GetRoles()
		{
			try
			{
				await Task.CompletedTask;
				var roles = await _context.tbl_roles.ToListAsync();
				if (!roles.Any())
				{
					return new ApiResponse<List<RoleDTO>>
					{
						Success = false,
						Message = "Không có quyền nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<RoleDTO>>
				{
					Success = true,
					Message = "Lấy danh sách quyền thành công.",
					Data = roles.Select(x => _mapper.Map<RoleDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<RoleDTO>>
				{
					Success = false,
					Message = "RoleService - GetRoles: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<object>>> GetRolesMapUser()
		{
			try
			{
				await Task.CompletedTask;
				var rolesUser = await _context.tbl_user_roles.ToListAsync();
				if (!rolesUser.Any())
				{
					return new ApiResponse<List<object>>
					{
						Success = false,
						Message = "Không có bất kỳ người dùng nào có quyền.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<object>>
				{
					Success = true,
					Message = "Lấy danh sách liên kết quyền thành công.",
					Data = rolesUser.Select(x => _mapper.Map<object>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<object>>
				{
					Success = false,
					Message = "RoleService - GetRolesMapUser: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<RoleDTO>> UpdateRole(int roleID, UpdateRoleDTO role)
		{
			try
			{
				await Task.CompletedTask;

				if (roleID != role.RoleID)
				{
					return new ApiResponse<RoleDTO>
					{
						Success = false,
						Message = "Quyền không đúng.",
						Status = (int)HttpStatusCode.BadRequest
					};
				};

				var roleEntity = await _context.tbl_roles.FindAsync(roleID);

				if (roleEntity == null)
				{
					return new ApiResponse<RoleDTO>
					{
						Success = false,
						Message = "Không thể cập nhật vì quyền không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				};

				_mapper.Map(role, roleEntity);
				_context.tbl_roles.Update(roleEntity);
				await _context.SaveChangesAsync();

				return new ApiResponse<RoleDTO>
				{
					Success = true,
					Message = "Cập nhật quyền thành công.",
					Data = _mapper.Map<RoleDTO>(roleEntity),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<RoleDTO>
				{
					Success = false,
					Message = "RoleService - UpdateRole: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> CreateRolesMapUser(CreateRolesMapUserDTO rolesMapUser)
		{
			try
			{
				await Task.CompletedTask;

				var userRoleEntity = _mapper.Map<tbl_user_role>(rolesMapUser);
				await _context.tbl_user_roles.AddAsync(userRoleEntity);
				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Tạo quyền thành công.",
					Data = _mapper.Map<object>(userRoleEntity),
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<object>
				{
					Success = false,
					Message = "UserRoleService - UpdateRolesMapUser: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> DeleteRoleMapUser(int userRoleID)
		{
			try
			{
				await Task.CompletedTask;
				var userRole = await _context.tbl_user_roles.FindAsync(userRoleID);
				if (userRole == null)
					return new ApiResponse<object>
					{
						Success = false,
						Message = "Không tìm thấy quyền.",
						Status = (int)HttpStatusCode.OK
					};

				_context.tbl_user_roles.Remove(userRole);

				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Xóa liên kết thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<object>
				{
					Success = false,
					Message = "RoleService - DeleteRoleMapUser: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
