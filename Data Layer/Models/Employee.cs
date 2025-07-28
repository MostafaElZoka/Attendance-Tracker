
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
    public FullName FullName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    public int DepartmenttId { get; set; }
    public Department Department { get; set; }

}
