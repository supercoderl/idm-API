namespace IDM_API.Data.Proposal
{
	public class ProposalDTO
	{
		public int ProposalsID { get; set; }
		public string Title { get; set; }
		public Guid? UserID { get; set; }
		public string? Content { get; set; }
		public int? FileID { get; set; }
		public int Status { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public Guid CreatedBy { get; set; }
		public DateTime? UpdatedDateTime { get; set; }
		public Guid? UpdatedBy { get; set; }
	}
}
