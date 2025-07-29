using Business_Layer.DTOs;
using Data_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        public Task<EmployeeDto> GetEmployeeByIdAsync(int id);
        public Task AddEmployee(AddEmployeeDto employee);
        public Task UpdateEmployeeAsync(EmployeeDto updateEmployee);
        public Task DeleteEmployeeAsync(int id);
        public Task<AttendanceSummaryDto> GetAttendanceMonthSummaryAsync(int employeeId);
    }
}
