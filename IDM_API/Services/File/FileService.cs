using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.File;
using IDM_API.Data.Task;
using IDM_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;

namespace IDM_API.Services.File
{
	public class FileService : IFileService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _web;

		public FileService(IDMContext context, IMapper mapper, IWebHostEnvironment web)
        {
			_context = context;
			_mapper = mapper;
			_web = web;
		}

		public async Task<ApiResponse<FileDTO>> CreateFile(IFormFile file, CreateFileDTO newFile)
		{
			try
			{
				await Task.CompletedTask;
				newFile.FilePath = UploadFile(file, newFile.FileName);
				var fileEntity = _mapper.Map<tbl_file>(newFile);
				await _context.tbl_files.AddAsync(fileEntity);
				await _context.SaveChangesAsync();
				return new ApiResponse<FileDTO>
				{
					Success = true,
					Message = "Tạo tài liệu mới thành công.",
					Data = _mapper.Map<FileDTO>(fileEntity),
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<FileDTO>
				{
					Success = false,
					Message = "FileService - CreateFile: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> DeleteFile(int fileID)
		{
			try
			{
				await Task.CompletedTask;
				var file = await _context.tbl_files.FindAsync(fileID);
				if (file == null)
					return new ApiResponse<object>
					{
						Success = false,
						Message = "Không tìm thấy tài liệu.",
						Status = (int)HttpStatusCode.NotFound
					};

				_context.tbl_files.Remove(file);

				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Xóa tài liệu thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "DepartmentService - DeleteDepartment: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<FileDTO>> GetFileByID(int fileID)
		{
			try
			{
				await Task.CompletedTask;
				var file = await _context.tbl_tasks.FindAsync(fileID);
				if (file == null)
				{
					return new ApiResponse<FileDTO>
					{
						Success = false,
						Message = "Không tìm thấy tài liệu này.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<FileDTO>
				{
					Success = true,
					Message = "Đã tìm thấy tài liệu.",
					Data = _mapper.Map<FileDTO>(file),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<FileDTO>
				{
					Success = false,
					Message = "FileService - GetFile: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<FileDTO>>> GetFiles()
		{
			try
			{
				await Task.CompletedTask;
				var files = await _context.tbl_files.ToListAsync();
				if (!files.Any())
				{
					return new ApiResponse<List<FileDTO>>
					{
						Success = false,
						Message = "Không có tài liệu nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<FileDTO>>
				{
					Success = true,
					Message = "Lấy danh sách tài liệu thành công.",
					Data = files.Select(x => _mapper.Map<FileDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<FileDTO>>
				{
					Success = false,
					Message = "FileService - GetFiles: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<FileDTO>> UpdateFile(int fileID, UpdateFileDTO file)
		{
			try
			{
				await Task.CompletedTask;
				if (fileID != file.FileID)
				{
					return new ApiResponse<FileDTO>
					{
						Success = false,
						Message = "Tài liệu không hợp lệ.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var fileEntity = await _context.tbl_files.FindAsync(fileID);
				if (fileEntity == null)
				{
					return new ApiResponse<FileDTO>
					{
						Success = false,
						Message = "Tài liệu không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				_mapper.Map(file, fileEntity);
				_context.tbl_files.Update(fileEntity);
				await _context.SaveChangesAsync();
				return new ApiResponse<FileDTO>
				{
					Success = true,
					Message = "Cập nhật tài liệu thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<FileDTO>
				{
					Success = false,
					Message = "FileService - UpdateFile: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		private string UploadFile(IFormFile file, string fileName)
		{
			try
			{
				string path = Path.Combine(_web.WebRootPath, "Files", DateTime.Now.ToString("dd-MM-yyyy"));
				string extension = Path.GetExtension(file.FileName);

				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}

				var pathToSave = Path.Combine(path, fileName + extension);
				if(file.Length > 0)
				{
					using(var stream = new FileStream(pathToSave, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					return pathToSave;
				}
				return string.Empty;
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}
}
