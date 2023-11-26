using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.Approval;
using IDM_API.Entities;
using System.Net;

namespace IDM_API.Services.Approval
{
	public class ApprovalService : IApprovalService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;

		public ApprovalService(IDMContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public async Task<ApiResponse<ApprovalDTO>> ApproveProposal(int proposalApprovals, UpdateApprovalDTO approval)
		{
			try
			{
				await Task.CompletedTask;
				if(proposalApprovals != approval.ProposalApprovals)
				{
					return new ApiResponse<ApprovalDTO>
					{
						Success = false,
						Message = "Đề xuất không hợp lệ.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var approvalEntity = await _context.tbl_approvals.FindAsync(proposalApprovals);
				if(approvalEntity == null)
				{
					return new ApiResponse<ApprovalDTO>
					{
						Success = false,
						Message = "Đề xuất không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				_mapper.Map(approval, approvalEntity);
				_context.tbl_approvals.Update(approvalEntity);
				await _context.SaveChangesAsync();
				return new ApiResponse<ApprovalDTO>
				{
					Success = true,
					Message = approval.ApprovalStatus == -1 ? "Đã hủy đề xuất" : "Duyệt đề xuất thành công",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ApprovalDTO>
				{
					Success = false,
					Message = "ApprovalService - UpdateApproval: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<ApprovalDTO>> CreateAnApproval(int proposalID)
		{
			try
			{
				await Task.CompletedTask;
				ApprovalDTO newApproval = new ApprovalDTO
				{
					ProposalID = proposalID,
					ApprovalStatus = 0
				};
				await _context.tbl_approvals.AddAsync(_mapper.Map<tbl_approval>(newApproval));
				await _context.SaveChangesAsync();
				return new ApiResponse<ApprovalDTO>
				{ 
					Success = true,
					Message = "Tạo thông báo duyệt đề xuất thành công.",
					Data = newApproval,
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ApprovalDTO>
				{
					Success = false,
					Message = "ApprovalService - CreateApproval: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
