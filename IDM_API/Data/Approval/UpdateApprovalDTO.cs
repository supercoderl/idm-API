namespace IDM_API.Data.Approval
{
	public class UpdateApprovalDTO
	{
		public int ProposalApprovals { get; set; }
		public Guid? ApproverID { get; set; }
		public int? ApprovalStatus { get; set; }
		public DateTime? ApprovalDate { get; set; } = DateTime.Now;
	}
}
