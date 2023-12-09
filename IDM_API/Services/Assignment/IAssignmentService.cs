using IDM_API.Data;
using IDM_API.Data.Assignment;

namespace IDM_API.Services.Assignment
{
	public interface IAssignmentService
	{
		Task<ApiResponse<List<AssignmentDTO>>> GetAssignments();
		Task<ApiResponse<List<AssignmentDTO>>> GetAssignmentsForLecture(Guid userID);
		Task<ApiResponse<AssignmentDTO>> GetAssignmentByID(int assignmentID);
		Task<ApiResponse<AssignmentDTO>> CreateAssignment(CreateAssignmentDTO newAssignment);
		Task<ApiResponse<AssignmentDTO>> UpdateAssignment(int assignmentID, UpdateAssignmentDTO assignment);
		Task<ApiResponse<Object>> DeleteAssignment(int assignmentID);
	}
}
