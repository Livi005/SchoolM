namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ClassRoom;

public class ClassRoomMeanAmmount
{
    public Dictionary<int, Dictionary<string, int>> ClassRoomsAndMeans { get; set; } = new();
    public int AmmountOfMaintenance2yo { get; set; }
}