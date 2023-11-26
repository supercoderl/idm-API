using IDM_API.Data;
using IDM_API.Data.Menu;

namespace IDM_API.Services.Menu
{
	public interface IMenuService
	{
		Task<ApiResponse<List<MenuDTO>>> GetMenus();
		Task<ApiResponse<List<MenuDTO>>> GetMenusByRole(string? role);
		Task<ApiResponse<MenuDTO>> GetMenuByID(int menuID);
	}
}
