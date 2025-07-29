
using Data_Layer.Models;

namespace Business_Layer.DTOs;

public class UpdateOrAddAttendanceDto
{
    public int EmployeeId { get; set; }
    public AttendanceStatus Status { get; set; }
    public DateTime Date { get; set; }
}
