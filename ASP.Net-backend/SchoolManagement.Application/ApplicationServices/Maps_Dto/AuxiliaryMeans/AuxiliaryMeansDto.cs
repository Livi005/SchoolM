namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.AuxiliaryMeans;

public class AuxiliaryMeansDto
{
    public string NameMean { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsAviable { get; set; } = true;
    public bool IsActive { get; set; } = true;
}