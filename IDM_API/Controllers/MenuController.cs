using IDM_API.Services.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class MenuController : ControllerBase
	{
		private readonly IMenuService _menuService;

		public MenuController(IMenuService menuService)
        {
			_menuService = menuService;
		}

		[HttpGet("get-menus")]
		public async Task<IActionResult> GetMenus()
		{
			var result = await _menuService.GetMenus();
			return StatusCode(result.Status, result);
		}

		[HttpGet("get-menus-by-role")]
		public async Task<IActionResult> GetMenusByRole()
		{
			var role = User.FindFirst(ClaimTypes.Role)?.Value;
			var result = await _menuService.GetMenusByRole(role);
			return StatusCode(result.Status, result);
		}

		[HttpGet("get-menu-by-id")]
		public async Task<IActionResult> GetMenu(int menuID)
		{
			var result = await _menuService.GetMenuByID(menuID);
			return StatusCode(result.Status, result);
		}
    }
}
