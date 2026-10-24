using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.SubjectAuxMean;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class SubjectAuxMeanService
    : BaseService<SubjectAuxMean, SubjectAuxMeanDto, SubjectAuxMeanResponseDto>, ISubjectAuxMeanService
{
    public SubjectAuxMeanService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SubjectAuxMeanService> logger)
        : base(unitOfWork, mapper, logger) { }

    public override async Task<Result<SubjectAuxMeanResponseDto>> CreateAsync(
        SubjectAuxMeanDto dto, CancellationToken cancellationToken = default)
    {
        var subject = await UnitOfWork.Repository<Subject>().GetByIdAsync(dto.IdSub, cancellationToken);
        if (subject is null)
            return Result.NotFound<SubjectAuxMeanResponseDto>("Subject", dto.IdSub);

        var auxMean = await UnitOfWork.Repository<AuxiliaryMeans>()
            .GetByIdAsync(dto.IdAuxMean, cancellationToken);
        if (auxMean is null)
            return Result.NotFound<SubjectAuxMeanResponseDto>("AuxiliaryMeans", dto.IdAuxMean);

        var entity = new SubjectAuxMean
        {
            IdSub = dto.IdSub,
            IdAuxMean = dto.IdAuxMean,
            Subject = subject,
            AuxMean = auxMean
        };

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Mapper.Map<SubjectAuxMeanResponseDto>(entity));
    }
}