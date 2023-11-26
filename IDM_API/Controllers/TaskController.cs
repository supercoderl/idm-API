using IDM_API.Data.ScheduleData;
using IDM_API.Data.Task;
using IDM_API.Entities;
using IDM_API.Services.TaskWorking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDM_API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class TaskController : ControllerBase
	{
		private readonly ITaskService _taskService;

		public TaskController(ITaskService taskService)
        {
			_taskService = taskService;
		}

		[HttpGet("get-tasks")]
		public async Task<IActionResult> GetSchedules()
		{
			var result = await _taskService.GetTasks();
			return StatusCode(result.Status, result);
		}

		[HttpGet("get-task-by-id")]
		public async Task<IActionResult> GetScheduleByID(int scheduleID)
		{
			var result = await _taskService.GetTask(scheduleID);
			return StatusCode(result.Status, result);
		}

		[HttpPost("create-task")]
		public async Task<IActionResult> CreateSchedule(TaskDTO newTask)
		{
			var result = await _taskService.CreateTask(newTask);
			return StatusCode(result.Status, result);
		}

		[HttpPut("update-task")]
		public async Task<IActionResult> UpdateSchedule(int taskID, TaskDTO task)
		{
			var result = await _taskService.UpdateTask(taskID, task);
			return StatusCode(result.Status, result);
		}
	}
}
