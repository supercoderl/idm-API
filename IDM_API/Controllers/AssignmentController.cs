using AutoMapper;
using IDM_API.Data.Assignment;
using IDM_API.Services.Assignment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AssignmentController : ControllerBase
	{
		private readonly IAssignmentService _assignmentService;
		private readonly IMapper _mapper;

		public AssignmentController(IAssignmentService assignmentService, IMapper mapper)
        {
			_assignmentService = assignmentService;
			_mapper = mapper;
		}

		[HttpGet("get-assignments")]
		public async Task<IActionResult> GetAssignments()
		{
			var result = await _assignmentService.GetAssignments();
			return StatusCode(result.Status, result);
		}

		[HttpGet("get-assignment/{assignmentID}")]
		public async Task<IActionResult> GetAssignment(int assignmentID)
		{
			var result = await _assignmentService.GetAssignmentByID(assignmentID);
			return StatusCode(result.Status, result);
		}

		[HttpPost("create-assignment")]
		public async Task<IActionResult> CreateAssignment(CreateAssignmentDTO newAssignment)
		{
			if (User.FindFirstValue("UserID") is not null)
				newAssignment.CreatedBy = Guid.Parse(User.FindFirstValue("UserID"));
			var result = await _assignmentService.CreateAssignment(newAssignment);
			return StatusCode(result.Status, result);
		}

		[HttpPut("update-assignment/{assignmentID}")]
		public async Task<IActionResult> UpdateAssignment(int assignmentID, UpdateAssignmentDTO assignment)
		{
			if (User.FindFirstValue("UserID") is not null)
				assignment.UpdatedBy = Guid.Parse(User.FindFirstValue("UserID"));
			var result = await _assignmentService.UpdateAssignment(assignmentID, assignment);
			return StatusCode(result.Status, result);
		}

		[HttpDelete("delete-assignment/{assignmentID}")]
		public async Task<IActionResult> DeleteAssignment(int assignmentID)
		{
			var result = await _assignmentService.DeleteAssignment(assignmentID);
			return StatusCode(result.Status, result);
		}
	}
}
