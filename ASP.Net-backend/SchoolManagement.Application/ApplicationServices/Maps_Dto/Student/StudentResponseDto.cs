namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Student;

public class StudentResponseDto
{
    public int Id { get; set; }
    public int IdC { get; set; }
    public StudentDto Student { get; set; } = new();
}