using SchoolManagement.Application.ApplicationServices.Maps_Dto.Administrator;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IAdministratorService : IBaseService<AdministratorDto, AdministratorResponseDto>
{
    Task<Result<AdministratorCreateResponseDto>> CreateWithUserAsync(
        AdministratorDto dto,
        CancellationToken cancellationToken = default);
}