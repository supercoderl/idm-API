namespace IDM_API.Data.ScheduleData
{
	public class CreateScheduleDTO
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public Guid? Organizer { get; set; }
		public TimeSpan StartTime { get; set; }
		public Decimal Duration { get; set; }
		public bool Repeat { get; set; }
		public bool OnWorkingDay { get; set; }
		public string DayOfWeek { get; set; }
		public int WeekOfMonth { get; set; }
		public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
	}
}
