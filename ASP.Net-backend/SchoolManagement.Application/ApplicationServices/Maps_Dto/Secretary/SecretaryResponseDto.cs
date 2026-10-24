namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Secretary;

public class SecretaryResponseDto
{
    public int Id { get; set; }
    public SecretaryDto Secretary { get; set; } = new();
}