using IDM_API.Data.Proposal;
using IDM_API.Data;
using IDM_API.Data.Department;

namespace IDM_API.Services.Department
{
	public interface IDepartmentService
	{
		Task<ApiResponse<List<DepartmentDTO>>> GetDepartments();
		Task<ApiResponse<DepartmentDTO>> GetDepartmentByID(int departmentID);
		Task<ApiResponse<DepartmentDTO>> CreateDepartment(CreateDepartment newDepartment);
		Task<ApiResponse<DepartmentDTO>> UpdateDepartment(int departmentID, UpdateDepartmentDTO department);
		Task<ApiResponse<object>> DeleteDepartment(int departmentID);
	}
}
