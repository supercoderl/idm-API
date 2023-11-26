using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.Department;
using IDM_API.Data.Proposal;
using IDM_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IDM_API.Services.Department
{
	public class DepartmentService : IDepartmentService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;

		public DepartmentService(IDMContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}
		 
		public async Task<ApiResponse<DepartmentDTO>> CreateDepartment(CreateDepartment newDepartment)
		{
			try
			{
				await Task.CompletedTask;
				var department = _mapper.Map<tbl_department>(newDepartment);
				await _context.tbl_departments.AddAsync(department);
				await _context.SaveChangesAsync();
				return new ApiResponse<DepartmentDTO>
				{
					Success = true,
					Message = "Tạo phòng ban thành công.",
					Data = _mapper.Map<DepartmentDTO>(department),
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<DepartmentDTO>
				{
					Success = false,
					Message = "DepartmentService - CreateDepartment: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> DeleteDepartment(int departmentID)
		{
			try
			{
				await Task.CompletedTask;
				await _context.Database.ExecuteSqlInterpolatedAsync($"sp_deleteDepartment {departmentID}");

				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Xóa phòng ban thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "DepartmentService - DeleteDepartment: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<DepartmentDTO>> GetDepartmentByID(int departmentID)
		{
			try
			{
				await Task.CompletedTask;
				var department = await _context.tbl_departments.FindAsync(departmentID);
				if (department == null)
				{
					return new ApiResponse<DepartmentDTO>
					{
						Success = false,
						Message = "Phòng ban này không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<DepartmentDTO>
				{
					Success = true,
					Message = "Đã tải phòng ban xong.",
					Data = _mapper.Map<DepartmentDTO>(department),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<DepartmentDTO>
				{
					Success = false,
					Message = "DepartmentService - GetDepartmentByID: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<DepartmentDTO>>> GetDepartments()
		{
			try
			{
				await Task.CompletedTask;
				var departments = await _context.tbl_departments.ToListAsync();
				if (!departments.Any())
				{
					return new ApiResponse<List<DepartmentDTO>>
					{
						Success = false,
						Message = "Không có phòng ban nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<DepartmentDTO>>
				{
					Success = true,
					Message = "Lấy danh sách phòng ban thành công.",
					Data = departments.Select(x => _mapper.Map<DepartmentDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<DepartmentDTO>>
				{
					Success = false,
					Message = "DepartmentService - GetDepartments: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<DepartmentDTO>> UpdateDepartment(int departmentID, UpdateDepartmentDTO department)
		{
			try
			{
				await Task.CompletedTask;
				if (departmentID != department.DepartmentID)
				{
					return new ApiResponse<DepartmentDTO>
					{
						Success = false,
						Message = "Phòng ban không đúng.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var departmentEntity = await _context.tbl_departments.FindAsync(departmentID);

				if (departmentEntity == null)
				{
					return new ApiResponse<DepartmentDTO>
					{
						Success = false,
						Message = "Phòng ban không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				_mapper.Map(department, departmentEntity);
				_context.tbl_departments.Update(departmentEntity);
				await _context.SaveChangesAsync();

				return new ApiResponse<DepartmentDTO>
				{
					Success = true,
					Message = "Chỉnh sửa phòng ban thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<DepartmentDTO>
				{
					Success = false,
					Message = "DepartmentService - UpdateDepartment: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
