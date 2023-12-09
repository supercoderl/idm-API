using IDM_API.Data.ScheduleData;
using IDM_API.Services.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ScheduleController : ControllerBase
	{
		private readonly IScheduleService _scheduleService;

		public ScheduleController(IScheduleService scheduleService)
        {
			_scheduleService = scheduleService;
		}

		[HttpGet("get-schedules")]
		public async Task<IActionResult> GetSchedules()
		{
			var result = await _scheduleService.GetSchedules("asd");
			return StatusCode(result.Status, result);
		}

		[HttpGet("get-schedule-by-id")]
		public async Task<IActionResult> GetScheduleByID(int scheduleID)
		{
			var result = await _scheduleService.GetScheduleByID(scheduleID);
			return StatusCode(result.Status, result);
		}

		[HttpPost("create-schedule")]
		public async Task<IActionResult> CreateSchedule(CreateScheduleDTO newSchedule)
		{
			var result = await _scheduleService.CreateSchedule(newSchedule);
			return StatusCode(result.Status, result);
		}

		[HttpPut("update-schedule/{scheduleID}")]
		public async Task<IActionResult> UpdateSchedule(int scheduleID, UpdateScheduleDTO schedule)
		{
			var result = await _scheduleService.UpdateSchedule(scheduleID, schedule);
			return StatusCode(result.Status, result);
		}

		[HttpDelete("delete-schedule/{scheduleID}")]
		public async Task<IActionResult> DeleteSchedule(int scheduleID)
		{
			var result = await _scheduleService.DeleteSchedule(scheduleID);
			return StatusCode(result.Status, result);
		}
	}
}
