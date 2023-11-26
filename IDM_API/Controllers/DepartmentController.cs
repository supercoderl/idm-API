using IDM_API.Data.Department;
using IDM_API.Data.Task;
using IDM_API.Services.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class DepartmentController : ControllerBase
	{
		private readonly IDepartmentService _departmentService;

		public DepartmentController(IDepartmentService departmentService)
        {
			_departmentService = departmentService;
		}

		[HttpGet("get-departments")]
		public async Task<IActionResult> GetDepartments()
		{
			var result = await _departmentService.GetDepartments();
			return StatusCode(result.Status, result);
		}

		[HttpGet("get-department-by-id/{departmentID}")]
		public async Task<IActionResult> GetDepartmentByID(int departmentID)
		{
			var result = await _departmentService.GetDepartmentByID(departmentID);
			return StatusCode(result.Status, result);
		}

		[HttpPost("create-department")]
		public async Task<IActionResult> CreateDepartment(CreateDepartment newDepartment)
		{
			var result = await _departmentService.CreateDepartment(newDepartment);
			return StatusCode(result.Status, result);
		}

		[HttpPut("update-department/{departmentID}")]
		public async Task<IActionResult> UpdateSchedule(int departmentID, UpdateDepartmentDTO department)
		{
			var result = await _departmentService.UpdateDepartment(departmentID, department);
			return StatusCode(result.Status, result);
		}

		[HttpDelete("delete-department/{departmentID}")]
		public async Task<IActionResult> DeleteDepartment(int departmentID)
		{
			var result = await _departmentService.DeleteDepartment(departmentID);
			return StatusCode(result.Status, result);
		}
	}
}
