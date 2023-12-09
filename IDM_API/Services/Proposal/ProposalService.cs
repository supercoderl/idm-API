using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.Assignment;
using IDM_API.Data.Proposal;
using IDM_API.Entities;
using IDM_API.Services.Approval;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace IDM_API.Services.Proposal
{
	public class ProposalService : IProposalService
	{
		private readonly IDMContext _context;
		private readonly IApprovalService _approvalService;
		private readonly IMapper _mapper;
		private readonly string _indexPath = @"C:\Path\To\Your\Lucene\Index";

		public ProposalService(IDMContext context, IApprovalService approvalService, IMapper mapper)
        {
			_context = context;
			_approvalService = approvalService;
			_mapper = mapper;
		}

		public async Task<ApiResponse<ProposalDTO>> CreateProposal(CreateProposalDTO newProposal)
		{
			try
			{
				await Task.CompletedTask;
				var proposal = _mapper.Map<tbl_proposal>(newProposal);
				await _context.tbl_proposals.AddAsync(proposal);
				await _context.SaveChangesAsync();
				await _approvalService.CreateAnApproval(_mapper.Map<ProposalDTO>(proposal).ProposalsID);
				return new ApiResponse<ProposalDTO>
				{
					Success = true,
					Message = "Gửi đề xuất thành công.",
					Data = _mapper.Map<ProposalDTO>(proposal),
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ProposalDTO>
				{
					Success = false,
					Message = "ProposalService - CreateProposal: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> DeleteProposal(int proposalID)
		{
			try
			{
				await Task.CompletedTask;
				await _context.Database.ExecuteSqlInterpolatedAsync($"sp_deleteProposal {proposalID}");

				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Xóa đề xuất thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "ProposalService - DeleteProposal: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<ProposalDTO>> GetProposalByID(int proposalID)
		{
			try
			{
				await Task.CompletedTask;
				var proposal = await _context.tbl_proposals.FindAsync(proposalID);
				if (proposal == null)
				{
					return new ApiResponse<ProposalDTO>
					{
						Success = false,
						Message = "Đề xuất này không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<ProposalDTO>
				{
					Success = true,
					Message = "Đã tải đề xuất xong.",
					Data = _mapper.Map<ProposalDTO>(proposal),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ProposalDTO>
				{
					Success = false,
					Message = "ProposalService - GetProposalByID: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<ProposalDTO>>> GetProposals()
		{
			try
			{
				await Task.CompletedTask;
				var proposals = await _context.tbl_proposals.ToListAsync();
				if (!proposals.Any())
				{
					return new ApiResponse<List<ProposalDTO>>
					{
						Success = false,
						Message = "Không có đề xuất nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<ProposalDTO>>
				{
					Success = true,
					Message = "Lấy danh sách đề xuất thành công.",
					Data = proposals.Select(x => _mapper.Map<ProposalDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<ProposalDTO>>
				{
					Success = false,
					Message = "ProposalService - GetProposals: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<ProposalDTO>>> GetProposalsForLecture(Guid userID)
		{
			try
			{
				await Task.CompletedTask;
				var proposals = await _context.tbl_proposals.Where(x => x.CreatedBy == userID).ToListAsync();
				if (!proposals.Any())
				{
					return new ApiResponse<List<ProposalDTO>>
					{
						Success = false,
						Message = "Không có đề xuất nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<ProposalDTO>>
				{
					Success = true,
					Message = $"Lấy danh sách đề xuất của {userID} thành công.",
					Data = proposals.Select(x => _mapper.Map<ProposalDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<ProposalDTO>>
				{
					Success = false,
					Message = "ProposalService - GetProposalsForLecture: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<ProposalDTO>> UpdateProposal(int proposalID, UpdateProposalDTO proposal)
		{
			try
			{
				await Task.CompletedTask;
				if (proposalID != proposal.ProposalsID)
				{
					return new ApiResponse<ProposalDTO>
					{
						Success = false,
						Message = "Đề xuất không đúng.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var proposalEntity = await _context.tbl_proposals.FindAsync(proposalID);

				if (proposalEntity == null)
				{
					return new ApiResponse<ProposalDTO>
					{
						Success = false,
						Message = "Đề xuất không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				_mapper.Map(proposal, proposalEntity);
				_context.tbl_proposals.Update(proposalEntity);
				await _context.SaveChangesAsync();

				return new ApiResponse<ProposalDTO>
				{
					Success = true,
					Message = "Chỉnh sửa đề xuất thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<ProposalDTO>
				{
					Success = false,
					Message = "ProposalService - UpdateProposal: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
