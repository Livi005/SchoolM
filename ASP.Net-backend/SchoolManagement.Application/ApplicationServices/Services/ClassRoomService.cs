using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoom;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ClassRoomService
    : BaseService<ClassRoom, ClassRoomDto, ClassRoomResponseDto>, IClassRoomService
{
    public ClassRoomService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClassRoomService> logger)
        : base(unitOfWork, mapper, logger) { }

    public async Task<Result<ClassRoomMeanAmmount>> GetClassRoomsMeanAmmountAsync(
        CancellationToken cancellationToken = default)
    {
        var classRooms = await Repository.ListAsync(cancellationToken);

        var classRoomTechMeans = await UnitOfWork.Repository<ClassRoomTechMean>().Query()
            .ToListAsync(cancellationToken);
        var maintenances = await UnitOfWork.Repository<Maintenance>().Query()
            .ToListAsync(cancellationToken);
        var subjects = await UnitOfWork.Repository<Subject>().Query()
            .ToListAsync(cancellationToken);
        var subjectAuxMeans = await UnitOfWork.Repository<SubjectAuxMean>().Query()
            .ToListAsync(cancellationToken);

        var result = new Dictionary<int, Dictionary<string, int>>();

        foreach (var cr in classRooms)
        {
            var techMeanIds = classRoomTechMeans
                .Where(x => x.IdClassRoom == cr.IdClassR)
                .Select(x => x.IdTechMean)
                .ToHashSet();

            var techCount = maintenances
                .Count(m => m.TypeOfMean == MeanType.Technological
                            && m.IdTechMean.HasValue
                            && techMeanIds.Contains(m.IdTechMean.Value));

            var subjectIds = subjects
                .Where(s => s.IdClassRoom == cr.IdClassR)
                .Select(s => s.IdSub)
                .ToHashSet();

            var auxMeanIds = subjectAuxMeans
                .Where(sam => subjectIds.Contains(sam.IdSub))
                .Select(sam => sam.IdAuxMean)
                .ToHashSet();

            var auxCount = maintenances
                .Count(m => m.TypeOfMean == MeanType.Auxiliary
                            && m.IdAuxMean.HasValue
                            && auxMeanIds.Contains(m.IdAuxMean.Value));

            result[cr.IdClassR] = new Dictionary<string, int>
            {
                ["TechnologicalMean"] = techCount,
                ["AuxiliaryMean"] = auxCount
            };
        }

        var twoYearsAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-2));
        var totalLast2Years = maintenances.Count(m => m.MaintenanceDate >= twoYearsAgo);

        return Result.Success(new ClassRoomMeanAmmount
        {
            ClassRoomsAndMeans = result,
            AmmountOfMaintenance2yo = totalLast2Years
        });
    }

    public async Task<Result<ClassRoomTechMeanAmmount>> GetClassRoomTechAmmountAsync(
        CancellationToken cancellationToken = default)
    {
        var classRooms = await Repository.ListAsync(cancellationToken);
        var classRoomTechMeans = await UnitOfWork.Repository<ClassRoomTechMean>().Query()
            .ToListAsync(cancellationToken);
        var techMeans = await UnitOfWork.Repository<TechnologicalMeans>().Query()
            .ToListAsync(cancellationToken);
        var maintenances = await UnitOfWork.Repository<Maintenance>().Query()
            .ToListAsync(cancellationToken);

        var oneYearAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-1));
        var result = new Dictionary<int, float>();

        foreach (var cr in classRooms)
        {
            var techMeanIds = classRoomTechMeans
                .Where(x => x.IdClassRoom == cr.IdClassR)
                .Select(x => x.IdTechMean)
                .ToHashSet();

            var costs = maintenances
                .Where(m => m.IdTechMean.HasValue
                            && techMeanIds.Contains(m.IdTechMean.Value)
                            && m.MaintenanceDate >= oneYearAgo)
                .Select(m => m.Cost)
                .ToList();

            if (costs.Count > 2)
                result[cr.IdClassR] = (float)costs.Average();
        }

        return Result.Success(new ClassRoomTechMeanAmmount
        {
            ClassRoomAverageCost = result
        });
    }
}