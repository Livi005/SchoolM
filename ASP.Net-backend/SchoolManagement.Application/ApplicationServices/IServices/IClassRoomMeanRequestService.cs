using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomMeanRequest;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IClassRoomMeanRequestService
{
    Task<Result<CMRGetAviableOrNotResponseDto>> GetAviableClassRoomMeanAsync(
        ClassRoomMeanRequestGetAviableDto dto, CancellationToken cancellationToken = default);

    Task<Result<CMRGetAviableOrNotResponseDto>> GetNotAviableClassRoomMeanAsync(
        ClassRoomMeanRequestGetAviableDto dto, CancellationToken cancellationToken = default);

    Task<Result<ClassRoommeanRequestReserveResponseDto>> ReserveClassRoomAndMeanAsync(
        ClassRoomMeanRequestReserveDto dto, CancellationToken cancellationToken = default);
}