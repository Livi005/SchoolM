using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Administrator;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class AdministratorService
    : BaseService<Administrator, AdministratorDto, AdministratorResponseDto>, IAdministratorService
{
    private readonly IUserRegistrationService _userRegistration;
    private readonly IValidationService _validation;

    public AdministratorService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AdministratorService> logger,
        IUserRegistrationService userRegistration,
        IValidationService validation)
        : base(unitOfWork, mapper, logger)
    {
        _userRegistration = userRegistration;
        _validation = validation;
    }

    public override Task<Result<AdministratorResponseDto>> CreateAsync(
        AdministratorDto dto,
        CancellationToken cancellationToken = default)
        => throw new NotSupportedException("Usa CreateWithUserAsync para crear un administrador.");

    public async Task<Result<AdministratorCreateResponseDto>> CreateWithUserAsync(
        AdministratorDto dto,
        CancellationToken cancellationToken = default)
    {
        if (!_validation.IsValidName(dto.AdminName))
            return Result.Failure<AdministratorCreateResponseDto>(
                Error.Validation("AdminName es requerido."));

        var (user, password) = await _userRegistration.RegisterUserAsync(
            dto.AdminName, Roles.Admin, cancellationToken);

        var entity = Mapper.Map<Administrator>(dto);
        entity.UserId = user.Id;

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AdministratorCreateResponseDto
        {
            Id = entity.AdminId,
            Administrator = Mapper.Map<AdministratorDto>(entity),
            UserName = user.UserName ?? string.Empty,
            Password = password
        });
    }
}