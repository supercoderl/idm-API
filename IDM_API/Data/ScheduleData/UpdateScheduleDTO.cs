namespace IDM_API.Data.ScheduleData
{
	public class UpdateScheduleDTO
	{
		public int ScheduleID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Guid Organizer { get; set; }
		public TimeSpan StartTime { get; set; }
		public Decimal Duration { get; set; }
		public bool Repeat { get; set; }
		public bool OnWorkingDay { get; set; }
		public string DayOfWeek { get; set; }
		public int WeekOfMonth { get; set; }
		public DateTime? UpdatedDateTime { get; set; } = DateTime.Now;
	}
}
