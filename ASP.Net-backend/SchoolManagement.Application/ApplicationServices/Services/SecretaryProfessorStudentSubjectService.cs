using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.SecretaryProfessorStudentSubject;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class SecretaryProfessorStudentSubjectService
    : BaseService<SecretaryProfessorStudentSubject,
                   SecretaryProfessorStudentSubjectDto,
                   SecretaryProfessorStudentSubjectResponseDto>,
      ISecretaryProfessorStudentSubjectService
{
    public SecretaryProfessorStudentSubjectService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<SecretaryProfessorStudentSubjectService> logger)
        : base(unitOfWork, mapper, logger) { }

    public override async Task<Result<SecretaryProfessorStudentSubjectResponseDto>> CreateAsync(
        SecretaryProfessorStudentSubjectDto dto, CancellationToken cancellationToken = default)
    {
        var secretary = await UnitOfWork.Repository<Secretary>()
            .GetByIdAsync(dto.IdSec, cancellationToken);
        if (secretary is null)
            return Result.NotFound<SecretaryProfessorStudentSubjectResponseDto>(
                "Secretary", dto.IdSec);

        var evaluation = await UnitOfWork.Repository<ProfessorStudentSubject>()
            .GetByIdAsync(dto.IdProfStudSub, cancellationToken);
        if (evaluation is null)
            return Result.NotFound<SecretaryProfessorStudentSubjectResponseDto>(
                "ProfessorStudentSubject", dto.IdProfStudSub);

        var entity = new SecretaryProfessorStudentSubject
        {
            IdSec = dto.IdSec,
            IdProfStudSub = dto.IdProfStudSub,
            Secretary = secretary,
            Evaluation = evaluation
        };

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Mapper.Map<SecretaryProfessorStudentSubjectResponseDto>(entity));
    }

    public override async Task<Result<SecretaryProfessorStudentSubjectResponseDto>> UpdateAsync(
        int id, SecretaryProfessorStudentSubjectDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.NotFound<SecretaryProfessorStudentSubjectResponseDto>(
                "SecretaryProfessorStudentSubject", id);

        if (entity.IdSec != dto.IdSec)
        {
            var secretary = await UnitOfWork.Repository<Secretary>()
                .GetByIdAsync(dto.IdSec, cancellationToken);
            if (secretary is null)
                return Result.NotFound<SecretaryProfessorStudentSubjectResponseDto>(
                    "Secretary", dto.IdSec);

            entity.IdSec = dto.IdSec;
            entity.Secretary = secretary;
        }

        if (entity.IdProfStudSub != dto.IdProfStudSub)
        {
            var evaluation = await UnitOfWork.Repository<ProfessorStudentSubject>()
                .GetByIdAsync(dto.IdProfStudSub, cancellationToken);
            if (evaluation is null)
                return Result.NotFound<SecretaryProfessorStudentSubjectResponseDto>(
                    "ProfessorStudentSubject", dto.IdProfStudSub);

            entity.IdProfStudSub = dto.IdProfStudSub;
            entity.Evaluation = evaluation;
        }

        Repository.Update(entity);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Mapper.Map<SecretaryProfessorStudentSubjectResponseDto>(entity));
    }
}