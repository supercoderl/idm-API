using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.ScheduleData;
using IDM_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IDM_API.Services.Schedule
{
	public class ScheduleService : IScheduleService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;

		public ScheduleService(IDMContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public async Task<ApiResponse<ScheduleDTO>> CreateSchedule(CreateScheduleDTO schedule)
		{
			try
			{
				await Task.CompletedTask;
				var scheduleEntity = _mapper.Map<tbl_schedule>(schedule);
				await _context.tbl_schedules.AddAsync(scheduleEntity);
				await _context.SaveChangesAsync();
				return new ApiResponse<ScheduleDTO>
				{
					Success = true,
					Message = "Tạo lịch họp thành công.",
					Data = _mapper.Map<ScheduleDTO>(scheduleEntity),
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ScheduleDTO>
				{
					Success = false,
					Message = "ScheduleService - CreateSchedule: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> DeleteSchedule(int scheduleID)
		{
			try
			{
				await Task.CompletedTask;
				var schedule = await _context.tbl_schedules.FindAsync(scheduleID);
				if (schedule == null)
					return new ApiResponse<object>
					{
						Success = false,
						Message = "Không tìm thấy cuộc họp.",
						Status = (int)HttpStatusCode.NotFound
					};

				_context.tbl_schedules.Remove(schedule);

				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Xóa cuộc họp thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "ScheduleService - DeleteSchedule: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<ScheduleDTO>> GetScheduleByID(int scheduleID)
		{
			try
			{
				await Task.CompletedTask;
				var schedule = await _context.tbl_schedules.FindAsync(scheduleID);
				if (schedule == null)
				{
					return new ApiResponse<ScheduleDTO>
					{
						Success = false,
						Message = "Không có lịch họp.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<ScheduleDTO>
				{
					Success = true,
					Message = "Tìm thấy lịch họp.",
					Data = _mapper.Map<ScheduleDTO>(schedule),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ScheduleDTO>
				{
					Success = false,
					Message = "ScheduleService - GetScheduleByID: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<ScheduleDTO>>> GetSchedules()
		{
			try
			{
				await Task.CompletedTask;
				var schedules = await _context.tbl_schedules.ToListAsync();
				if(!schedules.Any()) 
				{
					return new ApiResponse<List<ScheduleDTO>>
					{
						Success = false,
						Message = "Không có lịch họp nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<ScheduleDTO>>
				{
					Success = true,
					Message = "Lấy danh sách lịch họp thành công.",
					Data = schedules.Select(x => _mapper.Map<ScheduleDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<ScheduleDTO>>
				{
					Success = false,
					Message = "ScheduleService - GetSchedules: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<ScheduleDTO>> UpdateSchedule(int scheduleID, UpdateScheduleDTO schedule)
		{
			try
			{
				await Task.CompletedTask;

				if(scheduleID != schedule.ScheduleID)
				{
					return new ApiResponse<ScheduleDTO>
					{
						Success = false,
						Message = "Lịch họp không đúng.",
						Status = (int)HttpStatusCode.BadRequest
					};
				};

				var scheduleEntity = await _context.tbl_schedules.FindAsync(scheduleID);

				if(scheduleEntity == null)
				{
					return new ApiResponse<ScheduleDTO>
					{
						Success = false,
						Message = "Không thể cập nhật vì lịch họp không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				};

				_mapper.Map(schedule, scheduleEntity);
				_context.tbl_schedules.Update(scheduleEntity);
				await _context.SaveChangesAsync();

				return new ApiResponse<ScheduleDTO>
				{
					Success = true,
					Message = "Cập nhật lịch họp thành công.",
					Data = _mapper.Map<ScheduleDTO>(scheduleEntity),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ScheduleDTO>
				{
					Success = false,
					Message = "ScheduleService - UpdateSchedule: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
