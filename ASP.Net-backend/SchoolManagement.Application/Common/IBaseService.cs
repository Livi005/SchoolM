namespace SchoolManagement.Application.Common;

public interface IBaseService<TDto, TResponseDto>
{
    Task<Result<IReadOnlyList<TResponseDto>>> ListAsync(CancellationToken cancellationToken = default);
    Task<Result<TResponseDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<TResponseDto>> CreateAsync(TDto dto, CancellationToken cancellationToken = default);
    Task<Result<TResponseDto>> UpdateAsync(int id, TDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
}