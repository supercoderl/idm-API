using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.Menu;
using IDM_API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace IDM_API.Services.Menu
{
	public class MenuService : IMenuService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;

		public MenuService(IDMContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public async Task<ApiResponse<MenuDTO>> GetMenuByID(int menuID)
		{
			try
			{
				await Task.CompletedTask;
				var menu = await _context.tbl_menus.FindAsync(menuID);
				if(menu == null)
				{
					return new ApiResponse<MenuDTO>
					{
						Success = false,
						Message = "Không tìm thấy menu này.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<MenuDTO>
				{
					Success = true,
					Message = "Đã tìm thấy menu.",
					Data = _mapper.Map<MenuDTO>(menu),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<MenuDTO>
				{
					Success = false,
					Message = "MenuService - GetMenuByID: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<MenuDTO>>> GetMenus()
		{
			try
			{
				await Task.CompletedTask;
				var menus = await _context.tbl_menus.ToListAsync();
				if(!menus.Any())
				{
					return new ApiResponse<List<MenuDTO>>()
					{
						Success = false,
						Message = "Không tồn tại menu nào",
						Status = (int)HttpStatusCode.NotFound
					};
				};

				return new ApiResponse<List<MenuDTO>>()
				{
					Success = true,
					Message = "Lấy danh sách menu thành công.",
					Data = menus.Select(x => _mapper.Map<MenuDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<MenuDTO>>
				{
					Success = false,
					Message = "MenuService - GetMenus: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<MenuDTO>>> GetMenusByRole(string? role)
		{
			try
			{
				await Task.CompletedTask;
				if (role == null)
				{
					return new ApiResponse<List<MenuDTO>>()
					{
						Success = false,
						Message = "Người dùng này không có quyền hạn.",
						Status = (int)HttpStatusCode.Forbidden
					};
				}

				var menus = from m in _context.tbl_menus
							join mr in _context.tbl_menu_roles on m.MenuID equals mr.MenuID
							join r in _context.tbl_roles on mr.RoleID equals r.RoleID
							where r.RoleName.Equals(role)
							orderby m.Priority ascending
							select m;

				return new ApiResponse<List<MenuDTO>>()
				{
					Success = true,
					Message = "Lấy danh sách menu thành công.",
					Data = menus.Select(x => _mapper.Map<MenuDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<MenuDTO>>
				{
					Success = false,
					Message = "MenuService - GetMenusByRole: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
