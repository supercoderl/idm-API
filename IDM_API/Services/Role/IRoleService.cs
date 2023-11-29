using IDM_API.Data.ScheduleData;
using IDM_API.Data;
using IDM_API.Data.Role;

namespace IDM_API.Services.Role
{
	public interface IRoleService
	{
		Task<ApiResponse<List<RoleDTO>>> GetRoles();
		Task<ApiResponse<RoleDTO>> CreateRole(CreateRoleDTO newRole);
		Task<ApiResponse<RoleDTO>> UpdateRole(int roleID, UpdateRoleDTO role);
		Task<ApiResponse<Object>> DeleteRole(int roleID);
		Task<ApiResponse<RoleDTO>> GetRoleByID(int roleID);
		Task<ApiResponse<List<object>>> GetRolesMapUser();
		Task<ApiResponse<object>> CreateRolesMapUser(CreateRolesMapUserDTO rolesMapUser);
		Task<ApiResponse<object>> DeleteRoleMapUser(int userRoleID);
	}
}
