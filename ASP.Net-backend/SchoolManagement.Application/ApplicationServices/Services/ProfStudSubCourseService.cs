using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfStudSubCourse;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ProfStudSubCourseService
    : BaseService<ProfStudSubCourse, ProfStudSubCourseDto, ProfStudSubCourseResponseDto>,
      IProfStudSubCourseService
{
    private readonly IValidationService _validation;
    private readonly IIdentityManager _identityManager;

    public ProfStudSubCourseService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ProfStudSubCourseService> logger,
        IValidationService validation,
        IIdentityManager identityManager)
        : base(unitOfWork, mapper, logger)
    {
        _validation = validation;
        _identityManager = identityManager;
    }

    public override async Task<Result<IReadOnlyList<ProfStudSubCourseResponseDto>>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await Repository.Query()
            .Include(x => x.Professor)
            .Include(x => x.Student)
            .Include(x => x.Subject)
            .Include(x => x.Course)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<ProfStudSubCourseResponseDto>>(
            items.Select(MapToResponse).ToList());
    }

    public override async Task<Result<ProfStudSubCourseResponseDto>> CreateAsync(
        ProfStudSubCourseDto dto, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsInRange(dto.Evaluation, 0, 10))
            return Result.Failure<ProfStudSubCourseResponseDto>(
                Error.Validation("Evaluation debe estar entre 0 y 10."));

        var professor = await UnitOfWork.Repository<Professor>()
            .GetByIdAsync(dto.IdProf, cancellationToken);
        if (professor is null)
            return Result.NotFound<ProfStudSubCourseResponseDto>("Professor", dto.IdProf);

        var student = await UnitOfWork.Repository<Student>()
            .GetByIdAsync(dto.IdStud, cancellationToken);
        if (student is null)
            return Result.NotFound<ProfStudSubCourseResponseDto>("Student", dto.IdStud);

        var subject = await UnitOfWork.Repository<Subject>()
            .GetByIdAsync(dto.IdSub, cancellationToken);
        if (subject is null)
            return Result.NotFound<ProfStudSubCourseResponseDto>("Subject", dto.IdSub);

        var course = await UnitOfWork.Repository<Course>()
            .GetByIdAsync(dto.IdCourse, cancellationToken);
        if (course is null)
            return Result.NotFound<ProfStudSubCourseResponseDto>("Course", dto.IdCourse);

        var entity = new ProfStudSubCourse
        {
            IdProf = dto.IdProf,
            IdStud = dto.IdStud,
            IdSub = dto.IdSub,
            IdCourse = dto.IdCourse,
            Evaluation = dto.Evaluation,
            Professor = professor,
            Student = student,
            Subject = subject,
            Course = course
        };

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(MapToResponse(entity));
    }

    public override async Task<Result<ProfStudSubCourseResponseDto>> UpdateAsync(
        int id, ProfStudSubCourseDto dto, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsInRange(dto.Evaluation, 0, 10))
            return Result.Failure<ProfStudSubCourseResponseDto>(
                Error.Validation("Evaluation debe estar entre 0 y 10."));

        var entity = await Repository.Query()
            .Include(x => x.Professor)
            .Include(x => x.Student)
            .Include(x => x.Subject)
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.IdProfStudSubCourse == id, cancellationToken);

        if (entity is null)
            return Result.NotFound<ProfStudSubCourseResponseDto>("ProfStudSubCourse", id);

        entity.Evaluation = dto.Evaluation;
        Repository.Update(entity);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(MapToResponse(entity));
    }

    public async Task<Result<IReadOnlyList<ProfStudSubCourseResponseDto>>> ListByProfessorAsync(
        ProfStudSubCourseConsultDto consultDto, CancellationToken cancellationToken = default)
    {
        var user = await _identityManager.FindByUserNameAsync(consultDto.UserName);
        if (user is null)
            return Result.NotFound<IReadOnlyList<ProfStudSubCourseResponseDto>>("User", consultDto.UserName);

        var professor = await UnitOfWork.Repository<Professor>().Query()
            .FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
        if (professor is null)
            return Result.NotFound<IReadOnlyList<ProfStudSubCourseResponseDto>>(
                "Professor", consultDto.UserName);

        var items = await Repository.Query()
            .Include(x => x.Professor)
            .Include(x => x.Student)
            .Include(x => x.Subject)
            .Include(x => x.Course)
            .Where(x => x.IdProf == professor.IdProf)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<ProfStudSubCourseResponseDto>>(
            items.Select(MapToResponse).ToList());
    }

    public async Task<Result<IReadOnlyList<ProfStudSubCourseConsultResponseDto>>> GetPendingEvaluationsAsync(
        string userName, CancellationToken cancellationToken = default)
    {
        var user = await _identityManager.FindByUserNameAsync(userName);
        if (user is null)
            return Result.NotFound<IReadOnlyList<ProfStudSubCourseConsultResponseDto>>("User", userName);

        var student = await UnitOfWork.Repository<Student>().Query()
            .FirstOrDefaultAsync(s => s.UserId == user.Id, cancellationToken);
        if (student is null)
            return Result.NotFound<IReadOnlyList<ProfStudSubCourseConsultResponseDto>>(
                "Student", userName);

        var course = await UnitOfWork.Repository<Course>()
            .GetByIdAsync(student.IdC, cancellationToken);
        if (course is null)
            return Result.NotFound<IReadOnlyList<ProfStudSubCourseConsultResponseDto>>(
                "Course", student.IdC);

        var studentSubjects = await UnitOfWork.Repository<StudentSubject>().Query()
            .Where(ss => ss.IdStud == student.IdStud)
            .ToListAsync(cancellationToken);

        var subjectIds = studentSubjects.Select(ss => ss.IdSub).ToHashSet();

        var professorSubjects = await UnitOfWork.Repository<ProfessorSubject>().Query()
            .Include(ps => ps.Professor)
            .Include(ps => ps.Subject)
            .Where(ps => subjectIds.Contains(ps.IdSub))
            .ToListAsync(cancellationToken);

        var alreadyEvaluated = (await UnitOfWork.Repository<ProfStudSubCourse>().Query()
                .Where(x => x.IdStud == student.IdStud && x.IdCourse == course.IdC)
                .Select(x => new { x.IdProf, x.IdSub })
                .ToListAsync(cancellationToken))
            .Select(x => (x.IdProf, x.IdSub))
            .ToHashSet();

        var pending = new List<ProfStudSubCourseConsultResponseDto>();

        foreach (var ps in professorSubjects)
        {
            if (alreadyEvaluated.Contains((ps.IdProf, ps.IdSub))) continue;

            pending.Add(new ProfStudSubCourseConsultResponseDto
            {
                IdProf = ps.IdProf,
                IdStud = student.IdStud,
                IdSub = ps.IdSub,
                IdCourse = course.IdC,
                ProfName = ps.Professor?.NameProf ?? string.Empty,
                SubjectName = ps.Subject?.NameSub ?? string.Empty,
                CourseName = course.CourseName
            });
        }

        return Result.Success<IReadOnlyList<ProfStudSubCourseConsultResponseDto>>(pending);
    }

    // ---------- Helper ----------

    private static ProfStudSubCourseResponseDto MapToResponse(ProfStudSubCourse x) => new()
    {
        IdProfStudSubCourse = x.IdProfStudSubCourse,
        IdProf = x.IdProf,
        IdStud = x.IdStud,
        IdSub = x.IdSub,
        IdCourse = x.IdCourse,
        Evaluation = x.Evaluation,
        StudentName = x.Student?.NameStud ?? string.Empty,
        ProfessorName = x.Professor?.NameProf ?? string.Empty,
        CourseName = x.Course?.CourseName ?? string.Empty,
        SubjectName = x.Subject?.NameSub ?? string.Empty
    };
}