using SchoolManagement.Application.ApplicationServices.Maps_Dto.Professor;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IProfessorService : IBaseService<ProfessorDto, ProfessorResponseDto>
{
    Task<Result<ProfessorCreateResponseDto>> CreateWithUserAsync(
        ProfessorDto dto, CancellationToken cancellationToken = default);

    Task<Result<ProfessorsEvaluations>> GetGoodProfessorsAsync(CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ProfessorsBadResponse>>> GetBadProfessorsAsync(CancellationToken cancellationToken = default);
}