using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Secretary;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class SecretaryService
    : BaseService<Secretary, SecretaryDto, SecretaryResponseDto>, ISecretaryService
{
    private readonly IUserRegistrationService _userRegistration;
    private readonly IValidationService _validation;

    public SecretaryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<SecretaryService> logger,
        IUserRegistrationService userRegistration,
        IValidationService validation)
        : base(unitOfWork, mapper, logger)
    {
        _userRegistration = userRegistration;
        _validation = validation;
    }

    public override Task<Result<SecretaryResponseDto>> CreateAsync(
        SecretaryDto dto, CancellationToken cancellationToken = default)
        => throw new NotSupportedException("Usa CreateWithUserAsync para crear un secretario.");

    public async Task<Result<SecretaryCreateResponseDto>> CreateWithUserAsync(
        SecretaryDto dto, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsValidName(dto.NameS))
            return Result.Failure<SecretaryCreateResponseDto>(
                Error.Validation("NameS es requerido."));

        try
        {
            var (user, password) = await _userRegistration.RegisterUserAsync(
                dto.NameS, Roles.Secretary, cancellationToken);

            var entity = Mapper.Map<Secretary>(dto);
            entity.UserId = user.Id;

            await Repository.AddAsync(entity, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new SecretaryCreateResponseDto
            {
                Id = entity.IdS,
                Secretary = Mapper.Map<SecretaryDto>(entity),
                UserName = user.UserName ?? string.Empty,
                Password = password
            });
        }
        catch (InvalidOperationException ex)
        {
            Logger.LogError(ex, "Error creando secretario {Name}", dto.NameS);
            return Result.Failure<SecretaryCreateResponseDto>(Error.Failure(ex.Message));
        }
    }
}