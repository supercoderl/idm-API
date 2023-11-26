using AutoMapper;
using IDM_API.Data;
using IDM_API.Data.Assignment;
using IDM_API.Data.Progress;
using IDM_API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IDM_API.Services.Assignment
{
	public class AssignmentService : IAssignmentService
	{
		private readonly IDMContext _context;
		private readonly IMapper _mapper;

		public AssignmentService(IDMContext context, IMapper mapper)
        {
			_context = context;
			_mapper = mapper;
		}

		public async Task<ApiResponse<AssignmentDTO>> CreateAssignment(CreateAssignmentDTO newAssignment)
		{
			try
			{
				await Task.CompletedTask;
				var assignment = _mapper.Map<tbl_assignment>(newAssignment);
				await _context.tbl_assignments.AddAsync(assignment);
				await _context.SaveChangesAsync();

				var progress = new ProgressDTO
				{
					ProgressID = 0,
					AssignmentID = assignment.AssignmentID,
					Status = 0,
					Date = DateTime.Now,
				};

				await _context.tbl_progresses.AddAsync(_mapper.Map<tbl_progress>(progress));

				await _context.SaveChangesAsync();
				return new ApiResponse<AssignmentDTO>
				{
					Success = true,
					Message = "Giao việc thành công.",
					Data = _mapper.Map<AssignmentDTO>(assignment),
					Status = (int)HttpStatusCode.Created
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<AssignmentDTO>
				{
					Success = false,
					Message = "AssignmentService - CreateAssignment: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<object>> DeleteAssignment(int assignmentID)
		{
			try
			{
				await Task.CompletedTask;
				await _context.Database.ExecuteSqlInterpolatedAsync($"sp_deleteAssignment {assignmentID}");

				await _context.SaveChangesAsync();
				return new ApiResponse<object>
				{
					Success = true,
					Message = "Xóa bảng phân công thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<Object>
				{
					Success = false,
					Message = "AssignmentService - DeleteAssignment: " + ex,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<AssignmentDTO>> GetAssignmentByID(int assignmentID)
		{
			try
			{
				await Task.CompletedTask;
				var assignment = await _context.tbl_assignments.FindAsync(assignmentID);
				if (assignment == null)
				{
					return new ApiResponse<AssignmentDTO>
					{
						Success = false,
						Message = "Công việc này không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<AssignmentDTO>
				{
					Success = true,
					Message = "Đã tải công việc xong.",
					Data = _mapper.Map<AssignmentDTO>(assignment),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<AssignmentDTO>
				{
					Success = false,
					Message = "AssignmentService - GetAssignmentByID: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<List<AssignmentDTO>>> GetAssignments()
		{
			try
			{
				await Task.CompletedTask;
				var assignments = await _context.tbl_assignments.ToListAsync();
				if(!assignments.Any()) 
				{
					return new ApiResponse<List<AssignmentDTO>>
					{
						Success = false,
						Message = "Không có công việc nào.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				return new ApiResponse<List<AssignmentDTO>>
				{
					Success = true,
					Message = "Lấy danh sách công việc thành công.",
					Data = assignments.Select(x => _mapper.Map<AssignmentDTO>(x)).ToList(),
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<AssignmentDTO>>
				{
					Success = false,
					Message = "AssignmentService - GetAssignments: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		public async Task<ApiResponse<AssignmentDTO>> UpdateAssignment(int assignmentID, UpdateAssignmentDTO assignment)
		{
			try
			{
				await Task.CompletedTask;
				if(assignmentID != assignment.AssignmentID)
				{
					return new ApiResponse<AssignmentDTO>
					{
						Success = false,
						Message = "Công việc không đúng.",
						Status = (int)HttpStatusCode.BadRequest
					};
				}

				var assignmentEntity = await _context.tbl_assignments.FindAsync(assignmentID);

				if(assignmentEntity == null) 
				{
					return new ApiResponse<AssignmentDTO>
					{
						Success = false,
						Message = "Cong việc không tồn tại.",
						Status = (int)HttpStatusCode.NotFound
					};
				}

				_mapper.Map(assignment, assignmentEntity);
				_context.tbl_assignments.Update(assignmentEntity);
				await _context.SaveChangesAsync();

				return new ApiResponse<AssignmentDTO>
				{
					Success = true,
					Message = "Chỉnh sửa công việc thành công.",
					Status = (int)HttpStatusCode.OK
				};
			}
			catch (Exception ex)
			{
				return new ApiResponse<AssignmentDTO>
				{
					Success = false,
					Message = "AssignmentService - UpdateAssignment: " + ex.Message,
					Status = (int)HttpStatusCode.InternalServerError
				};
			}
		}
	}
}
