using AutoMapper;
using IDM_API.Data.Approval;
using IDM_API.Data.Assignment;
using IDM_API.Data.Department;
using IDM_API.Data.File;
using IDM_API.Data.Menu;
using IDM_API.Data.Progress;
using IDM_API.Data.Proposal;
using IDM_API.Data.Role;
using IDM_API.Data.ScheduleData;
using IDM_API.Data.Task;
using IDM_API.Data.UserData;
using IDM_API.Entities;

namespace IDM_API.Data.MapperData
{
	public class MapperProfile : Profile
	{
        public MapperProfile()
        {
            CreateMap<tbl_user, UserCreateDTO>();
			CreateMap<UserCreateDTO, tbl_user>();
			CreateMap<UserProfile, tbl_user>();
			CreateMap<tbl_user, UserProfile>();
			CreateMap<tbl_user, UserUpdateDTO>();
			CreateMap<UserUpdateDTO, tbl_user>();

			CreateMap<tbl_schedule, ScheduleDTO>();
			CreateMap<ScheduleDTO, tbl_schedule>();
			CreateMap<tbl_schedule, CreateScheduleDTO>();
			CreateMap<CreateScheduleDTO, tbl_schedule>();
			CreateMap<tbl_schedule, UpdateScheduleDTO>();
			CreateMap<UpdateScheduleDTO, tbl_schedule>();

			CreateMap<tbl_assignment, AssignmentDTO>();
			CreateMap<AssignmentDTO, tbl_assignment>();
			CreateMap<tbl_assignment, CreateAssignmentDTO>();
			CreateMap<CreateAssignmentDTO, tbl_assignment>();
			CreateMap<tbl_assignment, UpdateAssignmentDTO>();
			CreateMap<UpdateAssignmentDTO, tbl_assignment>();

			CreateMap<tbl_proposal, ProposalDTO>();
			CreateMap<ProposalDTO, tbl_proposal>();
			CreateMap<tbl_proposal, CreateProposalDTO>();
			CreateMap<CreateProposalDTO, tbl_proposal>();
			CreateMap<tbl_proposal, UpdateProposalDTO>();
			CreateMap<UpdateProposalDTO, tbl_proposal>();

			CreateMap<tbl_menu, MenuDTO>();
			CreateMap<MenuDTO, tbl_menu>();

			CreateMap<tbl_approval, ApprovalDTO>();
			CreateMap<ApprovalDTO, tbl_approval>();
			CreateMap<tbl_approval, UpdateApprovalDTO>();
			CreateMap<UpdateApprovalDTO, tbl_approval>();

			CreateMap<tbl_task, TaskDTO>();
			CreateMap<TaskDTO, tbl_task>();

			CreateMap<tbl_file, FileDTO>();
			CreateMap<FileDTO, tbl_file>();
			CreateMap<tbl_file, CreateFileDTO>();
			CreateMap<CreateFileDTO, tbl_file>();

			CreateMap<tbl_department, DepartmentDTO>();
			CreateMap<DepartmentDTO, tbl_department>();
			CreateMap<tbl_department, CreateDepartment>();
			CreateMap<CreateDepartment, tbl_department>();
			CreateMap<tbl_department, UpdateDepartmentDTO>();
			CreateMap<UpdateDepartmentDTO, tbl_department>();

			CreateMap<tbl_progress, ProgressDTO>();
			CreateMap<ProgressDTO, tbl_progress>();

			CreateMap<tbl_role, RoleDTO>();
			CreateMap<RoleDTO, tbl_role>();
			CreateMap<tbl_role, CreateRoleDTO>();
			CreateMap<CreateRoleDTO, tbl_role>();
			CreateMap<tbl_role, UpdateRoleDTO>();
			CreateMap<UpdateRoleDTO, tbl_role>();

			CreateMap<tbl_user_role, CreateRolesMapUserDTO>();
			CreateMap<CreateRolesMapUserDTO, tbl_user_role>();
		}
    }
}
