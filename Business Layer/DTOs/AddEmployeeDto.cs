
using Data_Layer.Models;
using System.ComponentModel.DataAnnotations;

namespace Business_Layer.DTOs;

public class AddEmployeeDto
{
    [Required]
    public FullName FullName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    public int DepartmentId { get; set; }
    public string DisplayFullName =>
    $"{FullName.FirstName} {FullName.SecondName} {FullName.ThirdName} {FullName.LastName}".Trim();
}
