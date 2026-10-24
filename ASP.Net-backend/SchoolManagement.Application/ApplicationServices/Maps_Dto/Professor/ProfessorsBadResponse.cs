namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Professor;

public class ProfessorsBadResponse
{
    public string NameProf { get; set; } = string.Empty;
    public bool UseAuxMean { get; set; }
    public DateTime PunishmentDate { get; set; }
    public List<float> Evals { get; set; } = new();
}