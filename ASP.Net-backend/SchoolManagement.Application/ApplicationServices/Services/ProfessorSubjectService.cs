using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorSubject;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ProfessorSubjectService
    : BaseService<ProfessorSubject, ProfessorSubjectDto, ProfessorSubjectResponseDto>, IProfessorSubjectService
{
    public ProfessorSubjectService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProfessorSubjectService> logger)
        : base(unitOfWork, mapper, logger) { }

    public override async Task<Result<ProfessorSubjectResponseDto>> CreateAsync(
        ProfessorSubjectDto dto, CancellationToken cancellationToken = default)
    {
        var professor = await UnitOfWork.Repository<Professor>()
            .GetByIdAsync(dto.IdProf, cancellationToken);
        if (professor is null)
            return Result.NotFound<ProfessorSubjectResponseDto>("Professor", dto.IdProf);

        var subject = await UnitOfWork.Repository<Subject>()
            .GetByIdAsync(dto.IdSub, cancellationToken);
        if (subject is null)
            return Result.NotFound<ProfessorSubjectResponseDto>("Subject", dto.IdSub);

        var entity = new ProfessorSubject
        {
            IdProf = dto.IdProf,
            IdSub = dto.IdSub,
            Professor = professor,
            Subject = subject
        };

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Mapper.Map<ProfessorSubjectResponseDto>(entity));
    }
}