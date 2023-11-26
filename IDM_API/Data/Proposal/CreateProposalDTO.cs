namespace IDM_API.Data.Proposal
{
	public class CreateProposalDTO
	{
		public string Title { get; set; }
		public Guid? UserID { get; set; }
		public string? Content { get; set; }
		public int? FileID { get; set; }
		public int Status { get; set; }
		public DateTime CreatedDateTime { get; set; } = DateTime.Now;
		public Guid CreatedBy { get; set; }
	}
}
