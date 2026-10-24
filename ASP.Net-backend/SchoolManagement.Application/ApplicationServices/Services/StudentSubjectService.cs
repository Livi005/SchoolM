using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.StudentSubject;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class StudentSubjectService
    : BaseService<StudentSubject, StudentSubjectDto, StudentSubjectResponseDto>, IStudentSubjectService
{
    public StudentSubjectService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StudentSubjectService> logger)
        : base(unitOfWork, mapper, logger) { }

    public override async Task<Result<StudentSubjectResponseDto>> CreateAsync(
        StudentSubjectDto dto, CancellationToken cancellationToken = default)
    {
        var student = await UnitOfWork.Repository<Student>()
            .GetByIdAsync(dto.IdStud, cancellationToken);
        if (student is null)
            return Result.NotFound<StudentSubjectResponseDto>("Student", dto.IdStud);

        var subject = await UnitOfWork.Repository<Subject>()
            .GetByIdAsync(dto.IdSub, cancellationToken);
        if (subject is null)
            return Result.NotFound<StudentSubjectResponseDto>("Subject", dto.IdSub);

        var entity = new StudentSubject
        {
            IdStud = dto.IdStud,
            IdSub = dto.IdSub,
            NJAbsents = dto.NJAbsents,
            Student = student,
            Subject = subject
        };

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Mapper.Map<StudentSubjectResponseDto>(entity));
    }
}