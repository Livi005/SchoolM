using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfStudSubCourse;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IProfStudSubCourseService
    : IBaseService<ProfStudSubCourseDto, ProfStudSubCourseResponseDto>
{
    Task<Result<IReadOnlyList<ProfStudSubCourseResponseDto>>> ListByProfessorAsync(
        ProfStudSubCourseConsultDto consultDto, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ProfStudSubCourseConsultResponseDto>>> GetPendingEvaluationsAsync(
        string userName, CancellationToken cancellationToken = default);
}