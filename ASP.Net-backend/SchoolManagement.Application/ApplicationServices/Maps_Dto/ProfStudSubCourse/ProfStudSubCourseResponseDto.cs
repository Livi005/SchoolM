namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfStudSubCourse;

public class ProfStudSubCourseResponseDto : ProfStudSubCourseDto
{
    public int IdProfStudSubCourse { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string ProfessorName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
}