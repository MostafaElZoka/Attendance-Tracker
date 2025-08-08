using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Data_Layer.Models;
using Data_Layer.Unit_Of_Work;
using Microsoft.EntityFrameworkCore;

namespace Business_Layer.Services;

internal class EmployeeService(IUnitOfWork unitOfWork) : IEmployeeService
{
    public async Task AddEmployee(AddEmployeeDto employee)
    {
        if (await unitOfWork.Employees.Exists(e => e.Email == employee.Email))
            throw new Exception("Email must be unique");

        if (!await unitOfWork.Departments.Exists(d => d.Id == employee.DepartmentId))
            throw new Exception($"Department ID {employee.DepartmentId} doesn't exist");

        //var names = new[] { employee.FullName.FirstName, employee.FullName.SecondName, employee.FullName.ThirdName, employee.FullName.LastName };

        //if (names.Any(n => n.Length < 2))
        //    throw new Exception("Each name part must be at least 2 characters");

        var emp = new Employee
        {
            FullName = employee.FullName,
            Email = employee.Email,
            DepartmentId = employee.DepartmentId
        };
        await unitOfWork.Employees.AddAsync(emp);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        var emp = await unitOfWork.Employees.GetByIdAsync(id);
        if (emp == null)
         throw new Exception($"Employee with ID {id} not found.");
        
        unitOfWork.Employees.Delete(emp);
        await unitOfWork.SaveChangesAsync();

    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await unitOfWork.Employees.GetAllAsync();

        return employees.Select(e=> new EmployeeDto
        {
            Code = e.Code,
            FullName = e.FullName,
            Email = e.Email,
            DepartmentId = e.DepartmentId
        });
    }

    public async Task<AttendanceSummaryDto> GetAttendanceMonthSummaryAsync(int employeeId)
    {
        var today = DateTime.Today;

        var firstDay = new DateTime(today.Year, today.Month, 1);

        var lastDay = firstDay.AddMonths(1).AddDays(-1);

        var emp = await unitOfWork.Employees.GetByIdAsync(employeeId);
        if (emp == null)
            throw new Exception($"Employee with ID {employeeId} not found.");

        var attendances = (await unitOfWork.Attendances.GetAllAsync())
            .Where(a => a.EmployeeId == emp.Code && a.Date >= firstDay && a.Date <= lastDay);

        var presents = attendances.Count(p => p.Status == AttendanceStatus.Present);
        var absents = attendances.Count(p => p.Status == AttendanceStatus.Absent);
        var total = absents + presents;
        var percentage = Math.Round((double)presents / total * 100, 2);

        return new AttendanceSummaryDto
        {
            Presents = presents,
            Absents = absents,
            Percentage = percentage
        };
    }

    public async Task<EmployeeDto> GetEmployeeDtoByIdAsync(int id)
    {
        var employee = await unitOfWork.Employees.GetByIdAsync(id);
        if (employee == null)
          throw new Exception($"Employee with ID {id} not found.");
        
        return new EmployeeDto
        {
            Code = employee.Code,
            FullName = employee.FullName,
            Email = employee.Email,
            DepartmentId = employee.DepartmentId

        };
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmpoyeesByDepartmentAsync(int departmenttId)
    {
        var employees = unitOfWork.Employees.GetAllQueryable()
            .Where(e => e.DepartmentId == departmenttId)
            .Select(e => new EmployeeDto
            {
                Code = e.Code,
                FullName = e.FullName,
                Email = e.Email,
                DepartmentId = e.DepartmentId
            });
        return await employees.ToListAsync();
    }

    public async Task UpdateEmployeeAsync(EmployeeDto updateEmployee)
    {
        var emp = await unitOfWork.Employees.GetByIdAsync(updateEmployee.Code);
        if (emp == null)
         throw new Exception($"Employee with ID {updateEmployee.Code} not found.");


        if (await unitOfWork.Employees.Exists(e => e.Email == updateEmployee.Email && e.Code != updateEmployee.Code))
            throw new Exception("Email must be unique");

        if (!await unitOfWork.Departments.Exists(d => d.Id == updateEmployee.DepartmentId))
            throw new Exception($"Department ID {updateEmployee.DepartmentId} doesn't exist");

        //var names = new[] { updateEmployee.FullName.FirstName, updateEmployee.FullName.SecondName, updateEmployee.FullName.ThirdName, updateEmployee.FullName.LastName };

        //if (names.Any(n => n.Length < 2))
        //    throw new Exception("Each name part must be at least 2 characters");

        emp.FullName = updateEmployee.FullName;
        emp.Email = updateEmployee.Email;
        emp.DepartmentId = updateEmployee.DepartmentId;
        unitOfWork.Employees.Update(emp);
        await unitOfWork.SaveChangesAsync();
    }
}
