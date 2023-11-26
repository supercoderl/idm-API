namespace IDM_API.Data.File
{
	public class CreateFileDTO
	{
		public string FileName { get; set; }
		public string? FilePath { get; set; }
		public string? DocumentType { get; set; }
		public string? FileType { get; set; }
		public int FileSize { get; set; }
		public DateTime? CreatedDateTime { get; set; } = DateTime.Now;
		public Guid? CreatedBy { get; set; }
	}
}
