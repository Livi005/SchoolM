using SchoolManagement.Domain.Common;

namespace SchoolManagement.Application.ApplicationServices.Maps_Dto.Maintenance;

public class MaintenanceDto
{
    public DateOnly MaintenanceDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public int Cost { get; set; }
    public MeanType TypeOfMean { get; set; } = MeanType.Technological;
    public int? IdAuxMean { get; set; }
    public int? IdTechMean { get; set; }
}