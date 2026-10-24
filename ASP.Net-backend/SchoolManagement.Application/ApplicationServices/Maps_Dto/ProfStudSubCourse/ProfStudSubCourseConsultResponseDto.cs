namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfStudSubCourse;

public class ProfStudSubCourseConsultResponseDto
{
    public int IdProf { get; set; }
    public int IdStud { get; set; }
    public int IdSub { get; set; }
    public int IdCourse { get; set; }
    public string ProfName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
}