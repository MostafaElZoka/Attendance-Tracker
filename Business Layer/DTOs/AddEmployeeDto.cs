
using Data_Layer.Models;
using System.ComponentModel.DataAnnotations;

namespace Business_Layer.DTOs;

public class AddEmployeeDto
{
    [Required]
    [RegularExpression(@"^([A-Za-z]{2,}\s){3}[A-Za-z]{2,}$",ErrorMessage ="You must enter your full name and each name must be longer than 1 charcter")]

    public string FullName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    public int DepartmentId { get; set; }
    //public string? DisplayFullName =>
    //$"{FullName.FirstName} {FullName.SecondName} {FullName.ThirdName} {FullName.LastName}".Trim();
}
