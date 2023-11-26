namespace IDM_API.Data.UserData
{
	public class UserUpdateDTO
	{
		public Guid UserID { get; set; }
		public string UsernameOrEmail { get; set; }
		public string Fullname { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int Status { get; set; }
		public int Priority { get; set; }
		public int? DepartmentID { get; set; }
		public DateTime? UpdatedDateTime { get; set; }
		public Guid? UpdatedBy { get; set; }
	}
}
