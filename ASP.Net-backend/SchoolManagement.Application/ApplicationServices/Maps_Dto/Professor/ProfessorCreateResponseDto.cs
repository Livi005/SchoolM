namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Professor;

public class ProfessorCreateResponseDto
{
    public int Id { get; set; }
    public ProfessorDto Professor { get; set; } = new();
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}