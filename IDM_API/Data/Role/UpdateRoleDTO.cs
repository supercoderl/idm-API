namespace IDM_API.Data.Role
{
	public class UpdateRoleDTO
	{
		public int RoleID { get; set; }
		public string RoleName { get; set; }
		public int RoleCode { get; set; }
		public bool IsActive { get; set; }
		public int Priority { get; set; }
		public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
	}
}
