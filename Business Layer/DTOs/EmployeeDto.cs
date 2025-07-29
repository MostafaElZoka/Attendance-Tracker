
using Data_Layer.Models;
using System.ComponentModel.DataAnnotations;

namespace Business_Layer.DTOs;

public class EmployeeDto:AddEmployeeDto
{
    public int Code { get; set; }

}
