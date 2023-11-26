using IDM_API.Data;
using IDM_API.Data.Proposal;

namespace IDM_API.Services.Proposal
{
	public interface IProposalService
	{
		Task<ApiResponse<List<ProposalDTO>>> GetProposals();
		Task<ApiResponse<ProposalDTO>> GetProposalByID(int proposalID);
		Task<ApiResponse<ProposalDTO>> CreateProposal(CreateProposalDTO newProposal);
		Task<ApiResponse<ProposalDTO>> UpdateProposal(int proposalID, UpdateProposalDTO proposal);
		Task<ApiResponse<object>> DeleteProposal(int proposalID);
	}
}
