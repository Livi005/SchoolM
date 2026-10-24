namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Professor;

public class ProfessorResponseDto
{
    public int Id { get; set; }
    public ProfessorDto Professor { get; set; } = new();
}