namespace IDM_API.Data.File
{
	public class UpdateFileDTO
	{
		public int FileID { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string DocumentType { get; set; }
		public string FileType { get; set; }
		public int FileSize { get; set; }
		public DateTime? UpdatedDateTime { get; set; } = DateTime.Now;
		public Guid? UpdatedBy { get; set; }
	}
}
