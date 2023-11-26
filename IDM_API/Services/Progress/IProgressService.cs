using IDM_API.Data;
using IDM_API.Data.Progress;

namespace IDM_API.Services.Progress
{
	public interface IProgressService
	{
		Task<ApiResponse<List<ProgressDTO>>> GetProgresses();
		Task<ApiResponse<ProgressDTO>> UpdateProgress(int progressID, ProgressDTO progressDTO);
	}
}
