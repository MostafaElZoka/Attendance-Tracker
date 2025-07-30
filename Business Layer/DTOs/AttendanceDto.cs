
using Data_Layer.Models;

namespace Business_Layer.DTOs;

public class AttendanceDto:UpdateOrAddAttendanceDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string DepartmentName { get; set; }   

}
