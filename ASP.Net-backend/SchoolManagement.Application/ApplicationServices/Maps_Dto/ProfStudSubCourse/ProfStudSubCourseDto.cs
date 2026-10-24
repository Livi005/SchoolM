namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.ProfStudSubCourse;

public class ProfStudSubCourseDto
{
    public int IdProf { get; set; }
    public int IdStud { get; set; }
    public int IdSub { get; set; }
    public int IdCourse { get; set; }
    /// <summary>Evaluación del alumno al profesor (0–10).</summary>
    public int Evaluation { get; set; }
}