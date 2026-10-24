namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorStudentSubject;

public class ProfessorStudentSubjectResponseDto
{
    public int IdProfStudSub { get; set; }
    public int IdStud { get; set; }
    public int IdSub { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public string ProfessorName { get; set; } = string.Empty;
    public float StudentGrades { get; set; }
}