
using System.ComponentModel.DataAnnotations;

namespace Data_Layer.Models;

public class FullName
{
    [Required]
    [MinLength(2)]
    public string FirstName { get; set; }
    [Required]
    [MinLength(2)]
    public string SecondName { get; set; }
    [Required]
    [MinLength(2)]
    public string ThirdName { get; set; }
    [Required]
    [MinLength(2)]
    public string LastName { get; set; }

}
