namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Secretary;

public class SecretaryCreateResponseDto
{
    public int Id { get; set; }
    public SecretaryDto Secretary { get; set; } = new();
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}