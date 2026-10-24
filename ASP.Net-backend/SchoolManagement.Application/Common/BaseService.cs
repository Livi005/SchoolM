using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolManagement.Domain.Common;
using SchoolManagement.Infrastructure.Common.GenericInterface;

namespace SchoolManagement.Application.Common;

public abstract class BaseService<TEntity, TDto, TResponseDto> : IBaseService<TDto, TResponseDto>
    where TEntity : class, IEntity
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly IRepository<TEntity, int> Repository;
    protected readonly IMapper Mapper;
    protected readonly ILogger Logger;

    protected BaseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
    {
        UnitOfWork = unitOfWork;
        Repository = unitOfWork.Repository<TEntity>();
        Mapper = mapper;
        Logger = logger;
    }

    public virtual async Task<Result<IReadOnlyList<TResponseDto>>> ListAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.ListAsync(cancellationToken);
        var dtos = Mapper.Map<List<TResponseDto>>(entities);
        return Result.Success<IReadOnlyList<TResponseDto>>(dtos);
    }

    public virtual async Task<Result<TResponseDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.NotFound<TResponseDto>(typeof(TEntity).Name, id);

        return Result.Success(Mapper.Map<TResponseDto>(entity));
    }

    public virtual async Task<Result<TResponseDto>> CreateAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = Mapper.Map<TEntity>(dto);
        await Repository.AddAsync(entity, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Mapper.Map<TResponseDto>(entity));
    }

    public virtual async Task<Result<TResponseDto>> UpdateAsync(int id, TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.NotFound<TResponseDto>(typeof(TEntity).Name, id);

        Mapper.Map(dto, entity);
        Repository.Update(entity);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(Mapper.Map<TResponseDto>(entity));
    }

    public virtual async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is null)
            return Result.Failure(Error.NotFound(typeof(TEntity).Name, id));

        Repository.Remove(entity);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}