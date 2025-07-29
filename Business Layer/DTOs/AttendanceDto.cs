
using Data_Layer.Models;

namespace Business_Layer.DTOs;

public class AttendanceDto
{
    public int EmployeeId { get; set; }
    public string FullName { get; set; }
    public string DepartmentName { get; set; }   
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }

}
