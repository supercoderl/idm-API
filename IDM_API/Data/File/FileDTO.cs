namespace IDM_API.Data.File
{
	public class FileDTO
	{
		public int FileID { get; set; }
		public string FileName { get; set; }
		public string? FilePath { get; set; }
		public string? DocumentType { get; set; }
		public string? FileType { get; set; }
		public int FileSize { get; set; }
		public DateTime CreatedDateTime { get; set; }
		public Guid CreatedBy { get; set; }
		public DateTime? UpdatedDateTime { get; set; }
		public Guid? UpdatedBy { get; set; }
	}
}
