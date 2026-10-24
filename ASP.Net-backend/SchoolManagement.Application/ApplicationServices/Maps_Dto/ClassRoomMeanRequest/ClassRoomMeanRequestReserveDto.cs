namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoomMeanRequest;

public class ClassRoomMeanRequestReserveDto
{
    public string UserName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public Dictionary<string, int> ReserveMeans { get; set; } = new();
    public bool Reserve { get; set; }
}