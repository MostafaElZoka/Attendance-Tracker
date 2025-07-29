
namespace Business_Layer.DTOs;

public class AttendanceSummaryDto
{
    public int Presents { get; set; }
    public int Absents { get; set; }
    public double Percentage { get; set; }
}
