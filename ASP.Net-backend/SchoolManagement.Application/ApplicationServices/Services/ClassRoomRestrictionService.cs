using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomRestriction;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Domain.Relations;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class ClassRoomRestrictionService
    : BaseService<ClassRoomRestriction, ClassRoomRestrictionDto, ClassRoomRestrictionResponseDto>,
      IClassRoomRestrictionService
{
    public ClassRoomRestrictionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ClassRoomRestrictionService> logger)
        : base(unitOfWork, mapper, logger) { }

    public override async Task<Result<ClassRoomRestrictionResponseDto>> CreateAsync(
        ClassRoomRestrictionDto dto, CancellationToken cancellationToken = default)
    {
        var restriction = await UnitOfWork.Repository<Restriction>()
            .GetByIdAsync(dto.IdRest, cancellationToken);
        if (restriction is null)
            return Result.NotFound<ClassRoomRestrictionResponseDto>("Restriction", dto.IdRest);

        var classRoom = await UnitOfWork.Repository<ClassRoom>()
            .GetByIdAsync(dto.IdClassRoom, cancellationToken);
        if (classRoom is null)
            return Result.NotFound<ClassRoomRestrictionResponseDto>("ClassRoom", dto.IdClassRoom);

        var entity = new ClassRoomRestriction
        {
            IdRest = dto.IdRest,
            IdClassRoom = dto.IdClassRoom,
            Restriction = restriction,
            ClassRoom = classRoom
        };

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(Mapper.Map<ClassRoomRestrictionResponseDto>(entity));
    }
}