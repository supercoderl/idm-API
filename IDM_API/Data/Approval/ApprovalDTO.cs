namespace IDM_API.Data.Approval
{
	public class ApprovalDTO
	{
		public int ProposalApprovals { get; set; }
		public int ProposalID { get; set; }
		public Guid? ApproverID { get; set; }
		public int? ApprovalStatus { get; set; }
		public DateTime? ApprovalDate { get; set;}
	}
}
