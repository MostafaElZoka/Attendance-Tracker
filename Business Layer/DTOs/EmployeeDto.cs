
using Data_Layer.Models;
using System.ComponentModel.DataAnnotations;

namespace Business_Layer.DTOs;

public class EmployeeDto
{
    public int Code { get; set; }

    [Required]
    public FullName FullName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    public int DepartmentId { get; set; }
}
