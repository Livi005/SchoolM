using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomMeanRequest;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;
using SchoolManagement.Infrastructure.Identity;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ClassRoomMeanRequestService : IClassRoomMeanRequestService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityManager _identityManager;
    private readonly ILogger<ClassRoomMeanRequestService> _logger;

    public ClassRoomMeanRequestService(
        IUnitOfWork unitOfWork,
        IIdentityManager identityManager,
        ILogger<ClassRoomMeanRequestService> logger)
    {
        _unitOfWork = unitOfWork;
        _identityManager = identityManager;
        _logger = logger;
    }

    public async Task<Result<CMRGetAviableOrNotResponseDto>> GetAviableClassRoomMeanAsync(
        ClassRoomMeanRequestGetAviableDto dto, CancellationToken cancellationToken = default)
    {
        var professorResult = await FindProfessorByUsernameAsync(dto.UserName, cancellationToken);
        if (professorResult.IsFailure)
            return Result.Failure<CMRGetAviableOrNotResponseDto>(professorResult.Error);

        var professor = professorResult.Value;
        var subjects = await GetProfessorSubjectsAsync(professor.IdProf, cancellationToken);

        var result = new CMRGetAviableOrNotResponseDto();

        foreach (var subject in subjects)
        {
            var auxMeanIds = await _unitOfWork.Repository<SubjectAuxMean>().Query()
                .Where(sam => sam.IdSub == subject.IdSub)
                .Select(sam => sam.IdAuxMean)
                .ToListAsync(cancellationToken);

            var counts = await _unitOfWork.Repository<AuxiliaryMeans>().Query()
                .Where(am => auxMeanIds.Contains(am.IdMean) && am.IsAviable)
                .GroupBy(am => am.NameMean)
                .Select(g => new { Name = g.Key, Amount = g.Count() })
                .ToListAsync(cancellationToken);

            result.Data[subject.NameSub] = counts
                .ToDictionary(x => x.Name, x => x.Amount);
        }

        return Result.Success(result);
    }

    public async Task<Result<CMRGetAviableOrNotResponseDto>> GetNotAviableClassRoomMeanAsync(
        ClassRoomMeanRequestGetAviableDto dto, CancellationToken cancellationToken = default)
    {
        var professorResult = await FindProfessorByUsernameAsync(dto.UserName, cancellationToken);
        if (professorResult.IsFailure)
            return Result.Failure<CMRGetAviableOrNotResponseDto>(professorResult.Error);

        var professor = professorResult.Value;
        var subjects = await GetProfessorSubjectsAsync(professor.IdProf, cancellationToken);

        var result = new CMRGetAviableOrNotResponseDto();

        foreach (var subject in subjects)
        {
            if (subject.ClassRoom is null || !subject.ClassRoom.IsAviable) continue;

            var auxMeanIds = await _unitOfWork.Repository<SubjectAuxMean>().Query()
                .Where(sam => sam.IdSub == subject.IdSub)
                .Select(sam => sam.IdAuxMean)
                .ToListAsync(cancellationToken);

            var counts = await _unitOfWork.Repository<AuxiliaryMeans>().Query()
                .Where(am => auxMeanIds.Contains(am.IdMean) && !am.IsAviable)
                .GroupBy(am => am.NameMean)
                .Select(g => new { Name = g.Key, Amount = g.Count() })
                .ToListAsync(cancellationToken);

            result.Data[subject.NameSub] = counts
                .ToDictionary(x => x.Name, x => x.Amount);
        }

        return Result.Success(result);
    }

    public async Task<Result<ClassRoommeanRequestReserveResponseDto>> ReserveClassRoomAndMeanAsync(
        ClassRoomMeanRequestReserveDto dto, CancellationToken cancellationToken = default)
    {
        var subject = await _unitOfWork.Repository<Subject>().Query()
            .FirstOrDefaultAsync(s => s.NameSub == dto.SubjectName, cancellationToken);
        if (subject is null)
            return Result.NotFound<ClassRoommeanRequestReserveResponseDto>("Subject", dto.SubjectName);

        var classRoom = await _unitOfWork.Repository<ClassRoom>()
            .GetByIdAsync(subject.IdClassRoom, cancellationToken);
        if (classRoom is null)
            return Result.NotFound<ClassRoommeanRequestReserveResponseDto>("ClassRoom", subject.IdClassRoom);

        // Aula: reservar / liberar
        if (dto.Reserve && !classRoom.IsAviable)
            return Fail(classRoom.IdClassR, $"El aula {classRoom.IdClassR} ya está ocupada.");

        if (!dto.Reserve && classRoom.IsAviable)
            return Fail(classRoom.IdClassR, $"El aula {classRoom.IdClassR} ya está libre.");

        classRoom.IsAviable = !dto.Reserve;
        _unitOfWork.Repository<ClassRoom>().Update(classRoom);

        // Medios: reservar / liberar por nombre
        foreach (var (meanName, amount) in dto.ReserveMeans)
        {
            var means = await _unitOfWork.Repository<AuxiliaryMeans>().Query()
                .Where(am => am.NameMean == meanName && am.IsAviable == !dto.Reserve)
                .Take(amount)
                .ToListAsync(cancellationToken);

            if (means.Count < amount)
                return Fail(classRoom.IdClassR,
                    $"No hay suficientes '{meanName}' disponibles (pediste {amount}, hay {means.Count}).");

            foreach (var mean in means)
            {
                mean.IsAviable = !dto.Reserve;
                _unitOfWork.Repository<AuxiliaryMeans>().Update(mean);
            }
        }

        // Relación profesor-aula (solo al reservar)
        if (dto.Reserve)
        {
            var professorResult = await FindProfessorByUsernameAsync(dto.UserName, cancellationToken);
            if (professorResult.IsFailure)
                return Result.Failure<ClassRoommeanRequestReserveResponseDto>(professorResult.Error);

            var professor = professorResult.Value;
            var exists = await _unitOfWork.Repository<ProfessorClassRoom>().Query()
                .AnyAsync(pcr => pcr.IdProf == professor.IdProf && pcr.IdClassR == classRoom.IdClassR,
                    cancellationToken);

            if (!exists)
            {
                await _unitOfWork.Repository<ProfessorClassRoom>().AddAsync(new ProfessorClassRoom
                {
                    IdProf = professor.IdProf,
                    IdClassR = classRoom.IdClassR,
                    Professor = professor,
                    ClassRoom = classRoom
                }, cancellationToken);
            }

            if (dto.ReserveMeans.Count > 0)
            {
                professor.UseAuxMean = true;
                _unitOfWork.Repository<Professor>().Update(professor);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new ClassRoommeanRequestReserveResponseDto
        {
            UserName = dto.UserName,
            SubjectName = dto.SubjectName,
            ReserveMeans = dto.ReserveMeans,
            Reserve = dto.Reserve,
            ClassRoom = classRoom.IdClassR,
            Success = true,
            Message = "OK"
        });
    }

    // ---------- Helpers ----------

    private async Task<Result<Professor>> FindProfessorByUsernameAsync(
        string userName, CancellationToken cancellationToken)
    {
        var user = await _identityManager.FindByUserNameAsync(userName);
        if (user is null)
            return Result.Failure<Professor>(Error.NotFound("User", userName));

        var professor = await _unitOfWork.Repository<Professor>().Query()
            .FirstOrDefaultAsync(p => p.UserId == user.Id, cancellationToken);

        return professor is null
            ? Result.Failure<Professor>(Error.NotFound("Professor", userName))
            : Result.Success(professor);
    }

    private async Task<List<Subject>> GetProfessorSubjectsAsync(
        int professorId, CancellationToken cancellationToken)
    {
        var subjectIds = await _unitOfWork.Repository<ProfessorSubject>().Query()
            .Where(ps => ps.IdProf == professorId)
            .Select(ps => ps.IdSub)
            .ToListAsync(cancellationToken);

        return await _unitOfWork.Repository<Subject>().Query()
            .Where(s => subjectIds.Contains(s.IdSub))
            .Include(s => s.ClassRoom)
            .ToListAsync(cancellationToken);
    }

    private static Result<ClassRoommeanRequestReserveResponseDto> Fail(int classRoomId, string message)
        => Result.Failure<ClassRoommeanRequestReserveResponseDto>(
            Error.Conflict($"{message} (ClassRoom {classRoomId})"));
}