using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Student;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class StudentService
    : BaseService<Student, StudentDto, StudentResponseDto>, IStudentService
{
    private readonly IUserRegistrationService _userRegistration;
    private readonly IValidationService _validation;
    private readonly IAverageCalculator _average;

    public StudentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<StudentService> logger,
        IUserRegistrationService userRegistration,
        IValidationService validation,
        IAverageCalculator average)
        : base(unitOfWork, mapper, logger)
    {
        _userRegistration = userRegistration;
        _validation = validation;
        _average = average;
    }

    public override Task<Result<StudentResponseDto>> CreateAsync(
        StudentDto dto, CancellationToken cancellationToken = default)
        => throw new NotSupportedException("Usa CreateWithUserAsync para crear un estudiante.");

    public async Task<Result<StudentCreateResponseDto>> CreateWithUserAsync(
        StudentDto dto, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsValidName(dto.NameStud))
            return Result.Failure<StudentCreateResponseDto>(Error.Validation("NameStud es requerido."));

        if (!_validation.IsInRange(dto.Age, 0, 100))
            return Result.Failure<StudentCreateResponseDto>(Error.Validation("Age debe estar entre 0 y 100."));

        var course = await UnitOfWork.Repository<Course>().GetByIdAsync(dto.IdC, cancellationToken);
        if (course is null)
            return Result.NotFound<StudentCreateResponseDto>("Course", dto.IdC);

        try
        {
            var (user, password) = await _userRegistration.RegisterUserAsync(
                dto.NameStud, Roles.Student, cancellationToken);

            var entity = Mapper.Map<Student>(dto);
            entity.UserId = user.Id;
            entity.Course = course;
            entity.IdC = course.IdC;

            await Repository.AddAsync(entity, cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new StudentCreateResponseDto
            {
                Id = entity.IdStud,
                IdC = entity.IdC,
                Student = Mapper.Map<StudentDto>(entity),
                UserName = user.UserName ?? string.Empty,
                Password = password
            });
        }
        catch (InvalidOperationException ex)
        {
            Logger.LogError(ex, "Error creando estudiante {Name}", dto.NameStud);
            return Result.Failure<StudentCreateResponseDto>(Error.Failure(ex.Message));
        }
    }

    public override async Task<Result<StudentResponseDto>> UpdateAsync(
        int id, StudentDto dto, CancellationToken cancellationToken = default)
    {
        if (!_validation.IsValidName(dto.NameStud))
            return Result.Failure<StudentResponseDto>(Error.Validation("NameStud es requerido."));

        if (!_validation.IsInRange(dto.Age, 0, 100))
            return Result.Failure<StudentResponseDto>(Error.Validation("Age debe estar entre 0 y 100."));

        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.NotFound<StudentResponseDto>("Student", id);

        var course = await UnitOfWork.Repository<Course>().GetByIdAsync(dto.IdC, cancellationToken);
        if (course is null)
            return Result.NotFound<StudentResponseDto>("Course", dto.IdC);

        Mapper.Map(dto, entity);
        entity.Course = course;
        entity.IdC = course.IdC;
        Repository.Update(entity);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Mapper.Map<StudentResponseDto>(entity));
    }

    public async Task<Result<IReadOnlyList<StudentInfoBad>>> GetBadStudentsAsync(
        CancellationToken cancellationToken = default)
    {
        var students = await Repository.ListAsync(cancellationToken);
        var result = new List<StudentInfoBad>();

        foreach (var student in students)
        {
            var studentSubjects = await UnitOfWork.Repository<StudentSubject>().Query()
                .Where(ss => ss.IdStud == student.IdStud)
                .ToListAsync(cancellationToken);

            var badSubjectsCount = 0;
            foreach (var ss in studentSubjects)
            {
                var grades = await UnitOfWork.Repository<ProfessorStudentSubject>().Query()
                    .Where(pss => pss.IdStudSub == ss.IdStudSub)
                    .Select(pss => pss.StudentGrades)
                    .ToListAsync(cancellationToken);

                var avg = _average.GetAverage(grades);
                if (avg >= 0 && avg < 50) badSubjectsCount++;
            }

            if (badSubjectsCount < 2) continue;

            var professorEvals = await UnitOfWork.Repository<ProfStudSubCourse>().Query()
                .Where(e => e.IdStud == student.IdStud && e.IdCourse == student.IdC)
                .Select(e => e.Evaluation)
                .ToListAsync(cancellationToken);

            var courseName = (await UnitOfWork.Repository<Course>()
                .GetByIdAsync(student.IdC, cancellationToken))?.CourseName ?? string.Empty;

            result.Add(new StudentInfoBad
            {
                StudentId = student.IdStud,
                StudentName = student.NameStud,
                Curso = courseName,
                ProfessorsAvarageEvaluation = _average.GetAverage(professorEvals)
            });
        }

        return Result.Success<IReadOnlyList<StudentInfoBad>>(result);
    }
}