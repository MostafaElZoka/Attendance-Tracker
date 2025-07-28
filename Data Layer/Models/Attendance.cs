
using System.ComponentModel.DataAnnotations;

namespace Data_Layer.Models;

public class Attendance
{
    public int Id { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public AttendaceStatus Status { get; set; }
}

public enum AttendaceStatus
{
    Present,
    Absent
}
