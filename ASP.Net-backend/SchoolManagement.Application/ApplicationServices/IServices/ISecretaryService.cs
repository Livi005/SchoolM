using SchoolManagement.Application.ApplicationServices.Maps_Dto.Secretary;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface ISecretaryService : IBaseService<SecretaryDto, SecretaryResponseDto>
{
    Task<Result<SecretaryCreateResponseDto>> CreateWithUserAsync(
        SecretaryDto dto,
        CancellationToken cancellationToken = default);
}