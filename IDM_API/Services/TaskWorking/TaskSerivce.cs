using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.Task;
using IDM_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IDM_API.Services.TaskWorking
{
	public class TaskSerivce : ITaskService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;

		public TaskSerivce(IDMContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public async Task<ApiResponse<TaskDTO>> CreateTask(TaskDTO newTask)
		{
			try
			{
				await Task.CompletedTask;
				await _context.tbl_tasks.AddAsync(_mapper.Map<tbl_task>(newTask));
				await _context.SaveChangesAsync();
				return new ApiResponse<TaskDTO>
				{
					Success = true,
					Message = "Tạo công việc mới thành công.",
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<TaskDTO>
				{
					Success = false,
					Message = "TaskService - CreateTask: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<TaskDTO>> GetTask(int taskID)
		{
			try
			{
				await Task.CompletedTask;
				var task = await _context.tbl_tasks.FindAsync(taskID);
				if (task == null)
				{
					return new ApiResponse<TaskDTO>
					{
						Success = false,
						Message = "Không tìm thấy công việc này.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<TaskDTO>
				{
					Success = true,
					Message = "Đã tìm thấy công việc.",
					Data = _mapper.Map<TaskDTO>(task),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<TaskDTO>
				{
					Success = false,
					Message = "TaskService - GetTask: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<TaskDTO>>> GetTasks()
		{
			try
			{
				await Task.CompletedTask;
				var tasks = await _context.tbl_tasks.ToListAsync();
				if(!tasks.Any())
				{
					return new ApiResponse<List<TaskDTO>>
					{
						Success = false,
						Message = "Không có công việc nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<TaskDTO>>
				{
					Success = true,
					Message = "Lấy danh sách công việc thành công.",
					Data = tasks.Select(x => _mapper.Map<TaskDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<TaskDTO>>
				{
					Success = false,
					Message = "TaskService - GetTasks: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<TaskDTO>> UpdateTask(int taskID, TaskDTO task)
		{
			try
			{
				await Task.CompletedTask;
				if(taskID != task.TaskID)
				{
					return new ApiResponse<TaskDTO>
					{
						Success = false,
						Message = "Công việc không hợp lệ.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var taskEntity = await _context.tbl_tasks.FindAsync(taskID);
				if(taskEntity == null)
				{
					return new ApiResponse<TaskDTO>
					{
						Success = false,
						Message = "Công việc không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				_mapper.Map(task, taskEntity);
				_context.tbl_tasks.Update(taskEntity);
				await _context.SaveChangesAsync();
				return new ApiResponse<TaskDTO>
				{
					Success = true,
					Message = "Cập nhật công việc thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<TaskDTO>
				{
					Success = false,
					Message = "TaskService - UpdateTask: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
