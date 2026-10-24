namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Student;

public class StudentCreateResponseDto
{
    public int Id { get; set; }
    public int IdC { get; set; }
    public StudentDto Student { get; set; } = new();
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}