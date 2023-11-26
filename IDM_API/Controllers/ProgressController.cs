using IDM_API.Data.Progress;
using IDM_API.Services.Progress;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProgressController : ControllerBase
	{
		private readonly IProgressService _progressService;

		public ProgressController(IProgressService progressService)
        {
			_progressService = progressService;
		}

		[HttpGet("get-progresses")]
		public async Task<IActionResult> GetProgresses()
		{
			var result = await _progressService.GetProgresses();
			return StatusCode(result.Status, result);
		}

		[HttpPut("update-progress/{progressID}")]
		public async Task<IActionResult> UpdateProgress(int progressID, ProgressDTO progress)
		{
			var result = await _progressService.UpdateProgress(progressID, progress);
			return StatusCode(result.Status, result);
		}
    }
}
