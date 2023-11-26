namespace IDM_API.Data.Assignment
{
	public class AssignmentDTO
	{
		public int AssignmentID { get; set; }
		public Guid Employee { get; set; }
		public int TaskID { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public DateTime Deadline { get; set; }
		public int Status { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public Guid CreatedBy { get; set; }
		public DateTime? UpdatedDateTime { get; set; }
		public Guid? UpdatedBy { get; set; }
	}
}
