namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Professor;

public class ProfessorDto
{
    public string NameProf { get; set; } = string.Empty;
    public string Contract { get; set; } = string.Empty;
    public string Spec { get; set; } = string.Empty;
    public int Salary { get; set; }
    public bool IsDean { get; set; }
    public int LaboralExperience { get; set; }
    public bool UseAuxMean { get; set; }
}