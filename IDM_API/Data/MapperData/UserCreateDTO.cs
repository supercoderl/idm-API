namespace IDM_API.Data.MapperData
{
	public class UserCreateDTO
	{
		public Guid UserID { get; set; } = Guid.NewGuid();
		public string UsernameOrEmail { get; set; }
		public string PasswordHash { get; set; }
		public string Fullname { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int? DepartmentID { get; set; }
		public DateTime? CreatedDateTime { get; set; } = DateTime.Now;
		public Guid? CreatedBy { get; set; }
	}
}
