using IDM_API.Data;
using IDM_API.Data.Approval;

namespace IDM_API.Services.Approval
{
	public interface IApprovalService
	{
		Task<ApiResponse<ApprovalDTO>> CreateAnApproval(int proposalID);
		Task<ApiResponse<ApprovalDTO>> ApproveProposal(int proposalApprovals, UpdateApprovalDTO approval);
	}
}
