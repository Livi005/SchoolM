using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomTechMean;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ClassRoomTechMeanService
    : BaseService<ClassRoomTechMean, ClassRoomTechMeanDto, ClassRoomTechMeanResponseDto>,
      IClassRoomTechMeanService
{
    public ClassRoomTechMeanService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClassRoomTechMeanService> logger)
        : base(unitOfWork, mapper, logger) { }

    public override async Task<Result<IReadOnlyList<ClassRoomTechMeanResponseDto>>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await Repository.Query()
            .Include(x => x.TechnologicalMeans)
            .ToListAsync(cancellationToken);

        var dtos = items.Select(x =>
        {
            var dto = Mapper.Map<ClassRoomTechMeanResponseDto>(x);
            dto.TechName = x.TechnologicalMeans?.NameMean ?? string.Empty;
            return dto;
        }).ToList();

        return Result.Success<IReadOnlyList<ClassRoomTechMeanResponseDto>>(dtos);
    }

    public override async Task<Result<ClassRoomTechMeanResponseDto>> CreateAsync(
        ClassRoomTechMeanDto dto, CancellationToken cancellationToken = default)
    {
        var tech = await UnitOfWork.Repository<TechnologicalMeans>()
            .GetByIdAsync(dto.IdTechMean, cancellationToken);
        if (tech is null)
            return Result.NotFound<ClassRoomTechMeanResponseDto>("TechnologicalMeans", dto.IdTechMean);

        var classRoom = await UnitOfWork.Repository<ClassRoom>()
            .GetByIdAsync(dto.IdClassRoom, cancellationToken);
        if (classRoom is null)
            return Result.NotFound<ClassRoomTechMeanResponseDto>("ClassRoom", dto.IdClassRoom);

        var entity = new ClassRoomTechMean
        {
            IdClassRoom = dto.IdClassRoom,
            IdTechMean = dto.IdTechMean,
            ClassRoom = classRoom,
            TechnologicalMeans = tech
        };

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = Mapper.Map<ClassRoomTechMeanResponseDto>(entity);
        response.TechName = tech.NameMean;
        return Result.Success(response);
    }
}