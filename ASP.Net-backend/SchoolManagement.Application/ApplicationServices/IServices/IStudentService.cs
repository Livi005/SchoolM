using SchoolManagement.Application.ApplicationServices.Maps_Dto.Student;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IStudentService : IBaseService<StudentDto, StudentResponseDto>
{
    Task<Result<StudentCreateResponseDto>> CreateWithUserAsync(
        StudentDto dto, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<StudentInfoBad>>> GetBadStudentsAsync(CancellationToken cancellationToken = default);
}