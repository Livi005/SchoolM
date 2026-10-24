using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolManagement.Application.ApplicationServices.IServices;
using SchoolManagement.Application.ApplicationServices.Maps_Dto.Subject;
using SchoolManagement.Application.Common;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.ApplicationServices.Services;

public class SubjectService
    : BaseService<Subject, SubjectDto, SubjectResponseDto>, ISubjectService
{
    public SubjectService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SubjectService> logger)
        : base(unitOfWork, mapper, logger) { }

    public override async Task<Result<IReadOnlyList<SubjectResponseDto>>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        var subjects = await Repository.Query()
            .Include(s => s.ClassRoom)
            .ToListAsync(cancellationToken);

        var dtos = subjects.Select(s =>
        {
            var dto = Mapper.Map<SubjectResponseDto>(s);
            dto.ClassRoomLocation = s.ClassRoom?.Location ?? string.Empty;
            return dto;
        }).ToList();

        return Result.Success<IReadOnlyList<SubjectResponseDto>>(dtos);
    }

    public override async Task<Result<SubjectResponseDto>> CreateAsync(
        SubjectDto dto, CancellationToken cancellationToken = default)
    {
        var classRoom = await UnitOfWork.Repository<ClassRoom>()
            .GetByIdAsync(dto.IdClassRoom, cancellationToken);
        if (classRoom is null)
            return Result.NotFound<SubjectResponseDto>("ClassRoom", dto.IdClassRoom);

        var entity = Mapper.Map<Subject>(dto);
        entity.ClassRoom = classRoom;
        entity.IdClassRoom = classRoom.IdClassR;

        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        var response = Mapper.Map<SubjectResponseDto>(entity);
        response.ClassRoomLocation = classRoom.Location;
        return Result.Success(response);
    }
}