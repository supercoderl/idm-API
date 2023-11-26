using IDM_API.Data;
using IDM_API.Data.Task;

namespace IDM_API.Services.TaskWorking
{
	public interface ITaskService
	{
		Task<ApiResponse<List<TaskDTO>>> GetTasks();
		Task<ApiResponse<TaskDTO>> GetTask(int taskID);
		Task<ApiResponse<TaskDTO>> CreateTask(TaskDTO newTask);
		Task<ApiResponse<TaskDTO>> UpdateTask(int taskID, TaskDTO newTask);
	}
}
