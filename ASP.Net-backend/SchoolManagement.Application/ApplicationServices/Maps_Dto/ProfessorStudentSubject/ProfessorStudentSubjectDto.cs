namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfessorStudentSubject;

public class ProfessorStudentSubjectDto
{
    /// <summary>Nombre de usuario del profesor (o superadmin) que registra la nota.</summary>
    public string UserName { get; set; } = string.Empty;
    public int IdStud { get; set; }
    public int IdSub { get; set; }
    public float StudentGrades { get; set; }
}