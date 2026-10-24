namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Subject;

public class SubjectDto
{
    public string NameSub { get; set; } = string.Empty;
    public string StudyProgram { get; set; } = string.Empty;
    public int CourseLoad { get; set; }
    public int IdClassRoom { get; set; }
}