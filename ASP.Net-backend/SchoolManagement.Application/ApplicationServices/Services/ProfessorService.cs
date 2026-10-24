using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Professor;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ProfessorService
    : BaseService<Professor, ProfessorDto, ProfessorResponseDto>, IProfessorService
{
    private readonly IUserRegistrationService _userRegistration;
    private readonly IValidationService _validation;
    private readonly IAverageCalculator _average;

    public ProfessorService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ProfessorService> logger,
        IUserRegistrationService userRegistration,
        IValidationService validation,
        IAverageCalculator average)
        : base(unitOfWork, mapper, logger)
    {
        _userRegistration = userRegistration;
        _validation = validation;
        _average = average;
    }

    public override Task<Result<ProfessorResponseDto>> CreateAsync(
        ProfessorDto dto, CancellationToken cancellationToken = default)
        => throw new NotSupportedException("Usa CreateWithUserAsync para crear un profesor.");

    public async Task<Result<ProfessorCreateResponseDto>> CreateWithUserAsync(
        ProfessorDto dto, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsValidName(dto.NameProf))
            return Result.Failure<ProfessorCreateResponseDto>(Error.Validation("NameProf es requerido."));

        if (dto.Salary < 0)
            return Result.Failure<ProfessorCreateResponseDto>(Error.Validation("Salary no puede ser negativo."));

        try
        {
            var (user, password) = await _userRegistration.RegisterUserAsync(
                dto.NameProf, Roles.Professor, cancellationToken);

            var entity = Mapper.Map<Professor>(dto);
            entity.UserId = user.Id;

            await Repository.AddAsync(entity, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new ProfessorCreateResponseDto
            {
                Id = entity.IdProf,
                Professor = Mapper.Map<ProfessorDto>(entity),
                UserName = user.UserName ?? string.Empty,
                Password = password
            });
        }
        catch (InvalidOperationException ex)
        {
            Logger.LogError(ex, "Error creando profesor {Name}", dto.NameProf);
            return Result.Failure<ProfessorCreateResponseDto>(Error.Failure(ex.Message));
        }
    }

    public override async Task<Result<ProfessorResponseDto>> UpdateAsync(
        int id, ProfessorDto dto, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsValidName(dto.NameProf))
            return Result.Failure<ProfessorResponseDto>(Error.Validation("NameProf es requerido."));

        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.NotFound<ProfessorResponseDto>("Professor", id);

        var previousSalary = entity.Salary;
        Mapper.Map(dto, entity);
        Repository.Update(entity);

        if (previousSalary > entity.Salary)
        {
            await UnitOfWork.Repository<ProfessorPunishment>().AddAsync(new ProfessorPunishment
            {
                IdProf = entity.IdProf,
                Professor = entity,
                PunishmentDate = DateTime.UtcNow
            }, cancellationToken);
        }

        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Mapper.Map<ProfessorResponseDto>(entity));
    }

    public async Task<Result<ProfessorsEvaluations>> GetGoodProfessorsAsync(
        CancellationToken cancellationToken = default)
    {
        var professors = await Repository.ListAsync(cancellationToken);
        var result = new ProfessorsEvaluations();

        foreach (var professor in professors)
        {
            var evaluations = await UnitOfWork.Repository<ProfStudSubCourse>().Query()
                .Where(e => e.IdProf == professor.IdProf)
                .Select(e => e.Evaluation)
                .ToListAsync(cancellationToken);

            if (_average.GetAverage(evaluations) <= 8) continue;

            var subjectNames = await UnitOfWork.Repository<ProfessorSubject>().Query()
                .Where(ps => ps.IdProf == professor.IdProf)
                .Include(ps => ps.Subject)
                .Select(ps => ps.Subject!.NameSub)
                .ToListAsync(cancellationToken);

            result.ProfessorsAndSubjects[professor.NameProf] = subjectNames;
        }

        return Result.Success(result);
    }

    public async Task<Result<IReadOnlyList<ProfessorsBadResponse>>> GetBadProfessorsAsync(
        CancellationToken cancellationToken = default)
    {
        var punishments = await UnitOfWork.Repository<ProfessorPunishment>().Query()
            .ToListAsync(cancellationToken);

        var punishedIds = punishments.Select(p => p.IdProf).Distinct().ToList();
        var result = new List<ProfessorsBadResponse>();

        foreach (var professorId in punishedIds)
        {
            var professor = await Repository.GetByIdAsync(professorId, cancellationToken);
            if (professor is null) continue;

            var firstPunishmentDate = punishments
                .Where(p => p.IdProf == professorId)
                .Min(p => p.PunishmentDate);

            var evals = await UnitOfWork.Repository<ProfStudSubCourse>().Query()
                .Where(e => e.IdProf == professorId)
                .OrderBy(e => e.Evaluation)
                .Select(e => (float)e.Evaluation)
                .Take(3)
                .ToListAsync(cancellationToken);

            result.Add(new ProfessorsBadResponse
            {
                NameProf = professor.NameProf,
                UseAuxMean = professor.UseAuxMean,
                PunishmentDate = firstPunishmentDate,
                Evals = evals
            });
        }

        return Result.Success<IReadOnlyList<ProfessorsBadResponse>>(result);
    }
}