using IDM_API.Data.Approval;
using IDM_API.Data.Proposal;
using IDM_API.Services.Approval;
using IDM_API.Services.Proposal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IDM_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProposalController : ControllerBase
	{
		private readonly IProposalService _proposalService;
		private readonly IApprovalService _approvalService;

		public ProposalController(IProposalService proposalService, IApprovalService approvalService)
        {
			_proposalService = proposalService;
			_approvalService = approvalService;
		}

		[HttpGet("get-proposals")]
		public async Task<IActionResult> GetProposals()
		{
			var result = await _proposalService.GetProposals();
			return StatusCode(result.Status, result);
		}

		[HttpGet("get-proposals-for-lecture")]
		public async Task<IActionResult> GetProposalsForLecture()
		{
			if(User.FindFirstValue("UserID") is not null)
			{
				var result = await _proposalService.GetProposalsForLecture(Guid.Parse(User.FindFirstValue("UserID")));
				return StatusCode(result.Status, result);
			}
			return Unauthorized();
		}

		[HttpGet("get-proposal-by-id")]
		public async Task<IActionResult> GetProposalByID(int proposalID)
		{
			var result = await _proposalService.GetProposalByID(proposalID);
			return StatusCode(result.Status, result);
		}

		[HttpPost("create-proposal")]
		public async Task<IActionResult> CreateProposal(CreateProposalDTO newProposal)
		{
			if(User.FindFirstValue("UserID") is not null)
			{
				newProposal.UserID = Guid.Parse(User.FindFirstValue("UserID"));
				newProposal.CreatedBy = Guid.Parse(User.FindFirstValue("UserID"));
			}
			var result = await _proposalService.CreateProposal(newProposal);
			return StatusCode(result.Status, result);
		}

		[HttpPut("update-proposal/{proposalID}")]
		public async Task<IActionResult> UpdateProposal(int proposalID, UpdateProposalDTO proposal)
		{
			if (User.FindFirstValue("UserID") is not null)
				proposal.UpdatedBy = Guid.Parse(User.FindFirstValue("UserID"));
			var result = await _proposalService.UpdateProposal(proposalID, proposal);
			return StatusCode(result.Status, result);
		}

		[HttpPut("approve-proposal/{approvalID}")]
		public async Task<IActionResult> ApproveProposal(int approvalID, UpdateApprovalDTO approval)
		{
			var userID = User.FindFirstValue("UserID");
			approval.ApproverID = Guid.Parse(userID);
			var result = await _approvalService.ApproveProposal(approvalID, approval);
			return StatusCode(result.Status, result);
		}

		[HttpDelete("delete-proposal/{proposalID}")]
		public async Task<IActionResult> DeleteProposal(int proposalID)
		{
			var result = await _proposalService.DeleteProposal(proposalID);
			return StatusCode(result.Status, result);
		}
	}
}
