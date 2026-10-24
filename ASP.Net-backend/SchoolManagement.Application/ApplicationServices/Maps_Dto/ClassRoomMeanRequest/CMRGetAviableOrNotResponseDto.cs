namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomMeanRequest;

public class CMRGetAviableOrNotResponseDto
{
    public Dictionary<string, Dictionary<string, int>> Data { get; set; } = new();
}