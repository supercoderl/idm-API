using IDM_API.Data;
using IDM_API.Data.File;

namespace IDM_API.Services.File
{
	public interface IFileService
	{
		Task<ApiResponse<List<FileDTO>>> GetFiles();
		Task<ApiResponse<FileDTO>> GetFileByID(int fileID);
		Task<ApiResponse<FileDTO>> CreateFile(IFormFile file, CreateFileDTO newFile);
		Task<ApiResponse<FileDTO>> UpdateFile(int fileID, UpdateFileDTO file);
		Task<ApiResponse<object>> DeleteFile(int fileID);
	}
}
