namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorClassRoom;

public class ProfessorClassRoomRequest
{
    public string NameProf { get; set; } = string.Empty;
    public string Spec { get; set; } = string.Empty;
    public Dictionary<int, Dictionary<string, string>> ClassRoomsAndMeans { get; set; } = new();
}