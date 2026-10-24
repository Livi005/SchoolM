using SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoom;
using SchoolManagement.Application.Common;

namespace SchoolManagement.Application.ApplicationServices.IServices;

public interface IClassRoomService : IBaseService<ClassRoomDto, ClassRoomResponseDto>
{
    Task<Result<ClassRoomMeanAmmount>> GetClassRoomsMeanAmmountAsync(CancellationToken cancellationToken = default);
    Task<Result<ClassRoomTechMeanAmmount>> GetClassRoomTechAmmountAsync(CancellationToken cancellationToken = default);
}