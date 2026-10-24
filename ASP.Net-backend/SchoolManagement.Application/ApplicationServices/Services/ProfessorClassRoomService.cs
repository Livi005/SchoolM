using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorClassRoom;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ProfessorClassRoomService : IProfessorClassRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProfessorClassRoomService> _logger;

    public ProfessorClassRoomService(IUnitOfWork unitOfWork, ILogger<ProfessorClassRoomService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ProfessorClassRoomGetSpecRequest>> GetSpecsAsync(
        CancellationToken cancellationToken = default)
    {
        var specs = await _unitOfWork.Repository<Professor>().Query()
            .Select(p => p.Spec)
            .Distinct()
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToListAsync(cancellationToken);

        return Result.Success(new ProfessorClassRoomGetSpecRequest { Spec = specs });
    }

    public async Task<Result<IReadOnlyList<ProfessorClassRoomRequest>>> GetProfessorsBySpecAsync(
        string spec, CancellationToken cancellationToken = default)
    {
        var professors = await _unitOfWork.Repository<Professor>().Query()
            .Where(p => p.Spec == spec)
            .ToListAsync(cancellationToken);

        var professorIds = professors.Select(p => p.IdProf).ToHashSet();

        var professorClassRooms = await _unitOfWork.Repository<ProfessorClassRoom>().Query()
            .Where(pcr => professorIds.Contains(pcr.IdProf))
            .Include(pcr => pcr.ClassRoom)
            .ToListAsync(cancellationToken);

        var classRoomIds = professorClassRooms.Select(pcr => pcr.IdClassR).Distinct().ToHashSet();

        var classRoomTechMeans = await _unitOfWork.Repository<ClassRoomTechMean>().Query()
            .Where(crtm => classRoomIds.Contains(crtm.IdClassRoom))
            .Include(crtm => crtm.TechnologicalMeans)
            .ToListAsync(cancellationToken);

        var result = new List<ProfessorClassRoomRequest>();

        foreach (var professor in professors)
        {
            var pcs = professorClassRooms.Where(pcr => pcr.IdProf == professor.IdProf).ToList();
            if (pcs.Count == 0) continue;

            var request = new ProfessorClassRoomRequest
            {
                NameProf = professor.NameProf,
                Spec = professor.Spec
            };

            foreach (var pc in pcs)
            {
                var means = classRoomTechMeans
                    .Where(crtm => crtm.IdClassRoom == pc.IdClassR)
                    .ToDictionary(
                        crtm => crtm.TechnologicalMeans?.NameMean ?? string.Empty,
                        crtm => crtm.TechnologicalMeans?.State ?? string.Empty);

                request.ClassRoomsAndMeans[pc.IdClassR] = means;
            }

            result.Add(request);
        }

        return Result.Success<IReadOnlyList<ProfessorClassRoomRequest>>(result);
    }
}