using IDM_API.Data.ScheduleData;
using IDM_API.Data;

namespace IDM_API.Services.Schedule
{
	public interface IScheduleService
	{
		Task<ApiResponse<List<ScheduleDTO>>> GetSchedules(string role);
		Task<ApiResponse<ScheduleDTO>> CreateSchedule(CreateScheduleDTO schedule);
		Task<ApiResponse<ScheduleDTO>> UpdateSchedule(int scheduleID, UpdateScheduleDTO schedule);
		Task<ApiResponse<Object>> DeleteSchedule(int scheduleID);
		Task<ApiResponse<ScheduleDTO>> GetScheduleByID(int scheduleID);
	}
}
