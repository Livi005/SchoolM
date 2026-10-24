namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Subject;

public class SubjectResponseDto : SubjectDto
{
    public int IdSub { get; set; }
    public string ClassRoomLocation { get; set; } = string.Empty;
}