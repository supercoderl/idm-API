using IDM_API.Data.File;
using IDM_API.Services.File;
using IDM_API.Services.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class FileController : ControllerBase
	{
		private readonly IFileService _fileService;

		public FileController(IFileService fileService)
        {
			_fileService = fileService;
		}

		[HttpGet("get-files")]
		public async Task<IActionResult> GetFiles()
		{
			var result = await _fileService.GetFiles();
			return StatusCode(result.Status, result);
		}

		[HttpGet("get-file")]
		public async Task<IActionResult> GetFile(int fileID)
		{
			var result = await _fileService.GetFileByID(fileID);
			return StatusCode(result.Status, result);
		}

		[HttpPost("create-file")]
		public async Task<IActionResult> CreateFile(IFormFile file, [FromQuery]CreateFileDTO newFile)
		{
			if(User.FindFirstValue("UserID") is not null)
			{
				newFile.CreatedBy = Guid.Parse(User.FindFirstValue("UserID"));
			}

			var result = await _fileService.CreateFile(file, newFile);
			return StatusCode(result.Status, result);
		}

		[HttpPut("update-file")]
		public async Task<IActionResult> UpdateFile(int fileID, UpdateFileDTO file)
		{
			var result = await _fileService.UpdateFile(fileID, file);
			return StatusCode(result.Status, result);
		}

		[HttpDelete("delete-file/{fileID}")]
		public async Task<IActionResult> DeleteFile(int fileID)
		{
			var result = await _fileService.DeleteFile(fileID);
			return StatusCode(result.Status, result);
		}
	}
}
