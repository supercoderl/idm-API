namespace IDM_API.Data.Menu
{
	public class MenuDTO
	{
		public int MenuID { get; set; }
		public string Code { get; set; }
		public string MenuName { get; set; }
		public string MenuPath { get; set; }
		public string MenuUrl { get; set; }
		public string MenuIcon { get; set; }
		public int? MenuParentID { get; set; }
		public int? Priority { get; set; }
	}
}
