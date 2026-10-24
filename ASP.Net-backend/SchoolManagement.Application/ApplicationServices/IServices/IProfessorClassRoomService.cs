using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorClassRoom;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IProfessorClassRoomService
{
    Task<Result<ProfessorClassRoomGetSpecRequest>> GetSpecsAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProfessorClassRoomRequest>>> GetProfessorsBySpecAsync(
        string spec, CancellationToken cancellationToken = default);
}