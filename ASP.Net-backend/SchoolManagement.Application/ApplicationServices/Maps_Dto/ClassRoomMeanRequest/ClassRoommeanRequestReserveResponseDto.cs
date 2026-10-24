namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomMeanRequest;

public class ClassRoommeanRequestReserveResponseDto : ClassRoomMeanRequestReserveDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ClassRoom { get; set; }
}