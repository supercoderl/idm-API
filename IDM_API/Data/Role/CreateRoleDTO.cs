namespace IDM_API.Data.Role
{
	public class CreateRoleDTO
	{
		public string RoleName { get; set; }
		public int RoleCode { get; set; }
		public bool IsActive { get; set; }
		public int Priority { get; set; }
		public DateTime CreatedDateTime { get; set; } = DateTime.Now;
	}
}
