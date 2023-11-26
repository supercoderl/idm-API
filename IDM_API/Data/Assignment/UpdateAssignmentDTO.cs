namespace IDM_API.Data.Assignment
{
	public class UpdateAssignmentDTO
	{
		public int AssignmentID { get; set; }
		public Guid Employee { get; set; }
		public int TaskID { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public DateTime Deadline { get; set; }
		public int Status { get; set; }
		public DateTime? UpdatedDateTime { get; set; } = DateTime.Now;
		public Guid? UpdatedBy { get; set; }
	}
}
