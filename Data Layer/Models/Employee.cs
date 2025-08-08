
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Layer.Models;

[Index(nameof(Code), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Code {  get; set; }
    [Required]
    [RegularExpression(@"^([A-Za-z]{2,}\s){3}[A-Za-z]{2,}$")]
    public string FullName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }

}
