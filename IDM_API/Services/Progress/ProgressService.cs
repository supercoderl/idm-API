using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.Progress;
using IDM_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IDM_API.Services.Progress
{
	public class ProgressService : IProgressService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;

		public ProgressService(IDMContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public async Task<ApiResponse<List<ProgressDTO>>> GetProgresses()
		{
			try
			{
				await Task.CompletedTask;
				var progresses = await _context.tbl_progresses.ToListAsync();
				if(!progresses.Any())
				{
					return new ApiResponse<List<ProgressDTO>>
					{
						Success = false,
						Message = "Không có tiến độ nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}
				return new ApiResponse<List<ProgressDTO>>
				{
					Success = true,
					Message = "Lấy danh sách tiến độ thành công.",
					Data = progresses.Select(x => _mapper.Map<ProgressDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<ProgressDTO>>
				{
					Success = false,
					Message = "ProgressService - GetProgresses: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<ProgressDTO>> UpdateProgress(int progressID, ProgressDTO progressDTO)
		{
			try
			{
				await Task.CompletedTask;
				if(progressDTO.ProgressID != progressID)
				{
					return new ApiResponse<ProgressDTO>
					{
						Success = false,
						Message = "Tiến độ không đúng.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var progress = await _context.tbl_progresses.FindAsync(progressID);
				if(progress == null)
				{
					return new ApiResponse<ProgressDTO>
					{
						Success = false,
						Message = "Không tìm thấy tiến độ.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				_mapper.Map(progressDTO, progress);
				_context.tbl_progresses.Update(progress);
				await _context.SaveChangesAsync();
				return new ApiResponse<ProgressDTO>
				{
					Success = true,
					Message = "Cập nhật tiến độ thành công.",
					Data = progressDTO,
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ProgressDTO>
				{
					Success = false,
					Message = "ProgressService - UpdateProgress: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
