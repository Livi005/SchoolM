namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Administrator;

public class AdministratorCreateResponseDto
{
    public int Id { get; set; }
    public AdministratorDto Administrator { get; set; } = new();
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}