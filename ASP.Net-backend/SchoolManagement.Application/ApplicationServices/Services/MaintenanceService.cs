using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Maintenance;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class MaintenanceService
    : BaseService<Maintenance, MaintenanceDto, MaintenanceResponseDto>, IMaintenanceService
{
    public MaintenanceService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MaintenanceService> logger)
        : base(unitOfWork, mapper, logger) { }

    public override async Task<Result<IReadOnlyList<MaintenanceResponseDto>>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await Repository.Query()
            .Include(m => m.AuxMean)
            .Include(m => m.TechnologicalMean)
            .ToListAsync(cancellationToken);

        var dtos = items.Select(m =>
        {
            var dto = Mapper.Map<MaintenanceResponseDto>(m);
            dto.MeanName = m.TypeOfMean == MeanType.Technological
                ? m.TechnologicalMean?.NameMean ?? string.Empty
                : m.AuxMean?.NameMean ?? string.Empty;
            return dto;
        }).ToList();

        return Result.Success<IReadOnlyList<MaintenanceResponseDto>>(dtos);
    }

    public override async Task<Result<MaintenanceResponseDto>> CreateAsync(
        MaintenanceDto dto, CancellationToken cancellationToken = default)
    {
        string meanName;

        if (dto.TypeOfMean == MeanType.Technological)
        {
            if (dto.IdTechMean is null)
                return Result.Failure<MaintenanceResponseDto>(
                    Error.Validation("IdTechMean es requerido cuando TypeOfMean es Technological."));

            var tech = await UnitOfWork.Repository<TechnologicalMeans>()
                .GetByIdAsync(dto.IdTechMean.Value, cancellationToken);
            if (tech is null)
                return Result.NotFound<MaintenanceResponseDto>("TechnologicalMeans", dto.IdTechMean.Value);

            meanName = tech.NameMean;
        }
        else
        {
            if (dto.IdAuxMean is null)
                return Result.Failure<MaintenanceResponseDto>(
                    Error.Validation("IdAuxMean es requerido cuando TypeOfMean es Auxiliary."));

            var aux = await UnitOfWork.Repository<AuxiliaryMeans>()
                .GetByIdAsync(dto.IdAuxMean.Value, cancellationToken);
            if (aux is null)
                return Result.NotFound<MaintenanceResponseDto>("AuxiliaryMeans", dto.IdAuxMean.Value);

            meanName = aux.NameMean;
        }

        var entity = Mapper.Map<Maintenance>(dto);
        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = Mapper.Map<MaintenanceResponseDto>(entity);
        response.MeanName = meanName;
        return Result.Success(response);
    }

    public override async Task<Result<MaintenanceResponseDto>> UpdateAsync(
        int id, MaintenanceDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.NotFound<MaintenanceResponseDto>("Maintenance", id);

        Mapper.Map(dto, entity);
        Repository.Update(entity);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = Mapper.Map<MaintenanceResponseDto>(entity);
        response.MeanName = dto.TypeOfMean == MeanType.Technological
            ? (await UnitOfWork.Repository<TechnologicalMeans>()
                .GetByIdAsync(dto.IdTechMean ?? 0, cancellationToken))?.NameMean ?? string.Empty
            : (await UnitOfWork.Repository<AuxiliaryMeans>()
                .GetByIdAsync(dto.IdAuxMean ?? 0, cancellationToken))?.NameMean ?? string.Empty;

        return Result.Success(response);
    }
}