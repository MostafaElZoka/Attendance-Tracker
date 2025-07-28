
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data_Layer.Models;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Code), IsUnique = true)]
public class Department
{
    public int Id { get; set; }
    private string _code;
    [Required]
    [MaxLength(4)]
    public string Code 
    {
        get => _code;
        set => _code = value.ToUpper();
    }
    [Required]
    [StringLength(maximumLength:50,MinimumLength =3)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Location { get; set; }
    public List<Employee> Employees { get; set; } = new List<Employee>();
}
