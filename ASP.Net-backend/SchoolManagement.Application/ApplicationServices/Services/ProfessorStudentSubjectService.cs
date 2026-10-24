using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorStudentSubject;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Student;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Subject;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ProfessorStudentSubjectService
    : BaseService<ProfessorStudentSubject, ProfessorStudentSubjectDto, ProfessorStudentSubjectResponseDto>,
      IProfessorStudentSubjectService
{
    private readonly IValidationService _validation;
    private readonly IIdentityManager _identityManager;

    public ProfessorStudentSubjectService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<ProfessorStudentSubjectService> logger,
        IValidationService validation,
        IIdentityManager identityManager)
        : base(unitOfWork, mapper, logger)
    {
        _validation = validation;
        _identityManager = identityManager;
    }

    public override async Task<Result<IReadOnlyList<ProfessorStudentSubjectResponseDto>>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await LoadAllWithIncludesAsync(cancellationToken);
        return Result.Success<IReadOnlyList<ProfessorStudentSubjectResponseDto>>(
            items.Select(MapToResponse).ToList());
    }

    public override async Task<Result<ProfessorStudentSubjectResponseDto>> CreateAsync(
        ProfessorStudentSubjectDto dto, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsInRange(dto.StudentGrades, 0, 100))
            return Result.Failure<ProfessorStudentSubjectResponseDto>(
                Error.Validation("StudentGrades debe estar entre 0 y 100."));

        if (string.IsNullOrWhiteSpace(dto.UserName))
            return Result.Failure<ProfessorStudentSubjectResponseDto>(
                Error.Validation("UserName es requerido."));

        // 1) Localizar al profesor por nombre de usuario.
        var user = await _identityManager.FindByUserNameAsync(dto.UserName);
        if (user is null)
            return Result.NotFound<ProfessorStudentSubjectResponseDto>("User", dto.UserName);

        var professor = await UnitOfWork.Repository<Professor>().Query()
            .FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
        if (professor is null)
            return Result.NotFound<ProfessorStudentSubjectResponseDto>("Professor", dto.UserName);

        // 2) Localizar la relación Estudiante-Asignatura.
        var studentSubject = await UnitOfWork.Repository<StudentSubject>().Query()
            .FirstOrDefaultAsync(ss => ss.IdStud == dto.IdStud && ss.IdSub == dto.IdSub, cancellationToken);
        if (studentSubject is null)
            return Result.NotFound<ProfessorStudentSubjectResponseDto>(
                "StudentSubject", $"student={dto.IdStud}, subject={dto.IdSub}");

        // 3) Crear la nota.
        var entity = new ProfessorStudentSubject
        {
            IdProf = professor.IdProf,
            IdStudSub = studentSubject.IdStudSub,
            Professor = professor,
            StudentSubject = studentSubject,
            StudentGrades = dto.StudentGrades
        };

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        // 4) Construir respuesta con nombres.
        var student = await UnitOfWork.Repository<Student>().GetByIdAsync(dto.IdStud, cancellationToken);
        var subject = await UnitOfWork.Repository<Subject>().GetByIdAsync(dto.IdSub, cancellationToken);

        return Result.Success(new ProfessorStudentSubjectResponseDto
        {
            IdProfStudSub = entity.IdProfStudSub,
            IdStud = dto.IdStud,
            IdSub = dto.IdSub,
            StudentName = student?.NameStud ?? string.Empty,
            SubjectName = subject?.NameSub ?? string.Empty,
            ProfessorName = professor.NameProf,
            StudentGrades = entity.StudentGrades
        });
    }

    public async Task<Result<IReadOnlyList<ProfessorStudentSubjectResponseDto>>> ListByProfessorUsernameAsync(
        string userName, CancellationToken cancellationToken = default)
    {
        var user = await _identityManager.FindByUserNameAsync(userName);
        if (user is null)
            return Result.NotFound<IReadOnlyList<ProfessorStudentSubjectResponseDto>>("User", userName);

        var professor = await UnitOfWork.Repository<Professor>().Query()
            .FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
        if (professor is null)
            return Result.NotFound<IReadOnlyList<ProfessorStudentSubjectResponseDto>>("Professor", userName);

        var items = await Repository.Query()
            .Include(x => x.Professor)
            .Include(x => x.StudentSubject)!.ThenInclude(ss => ss.Student)
            .Include(x => x.StudentSubject)!.ThenInclude(ss => ss.Subject)
            .Where(x => x.IdProf == professor.IdProf)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<ProfessorStudentSubjectResponseDto>>(
            items.Select(MapToResponse).ToList());
    }

    public async Task<Result> UpdateGradeAsync(
        int profStudSubId, float studentGrade, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsInRange(studentGrade, 0, 100))
            return Result.Failure(Error.Validation("La nota debe estar entre 0 y 100."));

        var entity = await Repository.GetByIdAsync(profStudSubId, cancellationToken);
        if (entity is null)
            return Result.Failure(Error.NotFound("ProfessorStudentSubject", profStudSubId));

        entity.StudentGrades = studentGrade;
        Repository.Update(entity);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<SubjectBriefDto>>> GetSubjectsOfProfessorAsync(
        string userName, CancellationToken cancellationToken = default)
    {
        var user = await _identityManager.FindByUserNameAsync(userName);
        if (user is null)
            return Result.NotFound<IReadOnlyList<SubjectBriefDto>>("User", userName);

        var professor = await UnitOfWork.Repository<Professor>().Query()
            .FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);
        if (professor is null)
            return Result.NotFound<IReadOnlyList<SubjectBriefDto>>("Professor", userName);

        var subjects = await UnitOfWork.Repository<ProfessorSubject>().Query()
            .Where(ps => ps.IdProf == professor.IdProf)
            .Select(ps => new SubjectBriefDto
            {
                IdSub = ps.IdSub,
                NameSub = ps.Subject != null ? ps.Subject.NameSub : string.Empty
            })
            .Distinct()
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<SubjectBriefDto>>(subjects);
    }

    public async Task<Result<IReadOnlyList<StudentBriefDto>>> GetStudentsForSubjectAsync(
        int subjectId, CancellationToken cancellationToken = default)
    {
        var students = await UnitOfWork.Repository<StudentSubject>().Query()
            .Where(ss => ss.IdSub == subjectId)
            .Select(ss => new StudentBriefDto
            {
                IdStud = ss.IdStud,
                NameStud = ss.Student != null ? ss.Student.NameStud : string.Empty
            })
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<StudentBriefDto>>(students);
    }

    // ---------- Helpers ----------

    private async Task<List<ProfessorStudentSubject>> LoadAllWithIncludesAsync(CancellationToken ct)
    {
        return await Repository.Query()
            .Include(x => x.Professor)
            .Include(x => x.StudentSubject)!.ThenInclude(ss => ss.Student)
            .Include(x => x.StudentSubject)!.ThenInclude(ss => ss.Subject)
            .ToListAsync(ct);
    }

    private static ProfessorStudentSubjectResponseDto MapToResponse(ProfessorStudentSubject x) => new()
    {
        IdProfStudSub = x.IdProfStudSub,
        IdStud = x.StudentSubject?.IdStud ?? 0,
        IdSub = x.StudentSubject?.IdSub ?? 0,
        StudentName = x.StudentSubject?.Student?.NameStud ?? string.Empty,
        SubjectName = x.StudentSubject?.Subject?.NameSub ?? string.Empty,
        ProfessorName = x.Professor?.NameProf ?? string.Empty,
        StudentGrades = x.StudentGrades
    };
}