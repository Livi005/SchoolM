using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorStudentSubject;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Student;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Subject;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IProfessorStudentSubjectService
    : IBaseService<ProfessorStudentSubjectDto, ProfessorStudentSubjectResponseDto>
{
    Task<Result<IReadOnlyList<ProfessorStudentSubjectResponseDto>>> ListByProfessorUsernameAsync(
        string userName, CancellationToken cancellationToken = default);

    Task<Result> UpdateGradeAsync(
        int profStudSubId, float studentGrade, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<SubjectBriefDto>>> GetSubjectsOfProfessorAsync(
        string userName, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<StudentBriefDto>>> GetStudentsForSubjectAsync(
        int subjectId, CancellationToken cancellationToken = default);
}