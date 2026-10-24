namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Administrator;

public class AdministratorResponseDto
{
    public int Id { get; set; }
    public AdministratorDto Administrator { get; set; } = new();
}