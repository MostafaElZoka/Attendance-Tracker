
using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Data_Layer.Models;
using Data_Layer.Unit_Of_Work;
using Microsoft.EntityFrameworkCore;

namespace Business_Layer.Services;

internal class AttendanceService(IUnitOfWork unitOfWork) : IAttendanceService
{
    public async Task AddAttendanceAsync(UpdateOrAddAttendanceDto attendanceDto)
    {
        if(attendanceDto.Date > DateTime.Today)
            throw new Exception("Attendance date cannot be in the future.");

        if(!await unitOfWork.Employees.Exists(e => e.Code == attendanceDto.EmployeeId))
            throw new Exception("Employee does not exist.");

        if (await unitOfWork.Attendances.Exists(a => a.EmployeeId == attendanceDto.EmployeeId && a.Date == attendanceDto.Date))
            throw new Exception("Attendance record for this employee on this date already exists.");

        var attendance = new Attendance
        {
            EmployeeId = attendanceDto.EmployeeId,
            Date = attendanceDto.Date.Date,
            Status = attendanceDto.Status
        };
        await unitOfWork.Attendances.AddAsync(attendance);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAttendance(int id)
    {
        var record = await unitOfWork.Attendances.GetByIdAsync(id);
        if (record == null)
            throw new Exception("Attendance record not found.");
         unitOfWork.Attendances.Delete(record);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task EditAttendance(int id,UpdateOrAddAttendanceDto attendanceDto)
    {
        var record = await unitOfWork.Attendances.GetByIdAsync(id);
        if (record == null)
            throw new Exception("Attendance record not found.");

        if (attendanceDto.Date > DateTime.Today)
            throw new Exception("Attendance date cannot be in the future.");

        //if (!await unitOfWork.Employees.Exists(e => e.Code == attendanceDto.EmployeeId))
        //    throw new Exception("Employee does not exist.");

        record.EmployeeId = attendanceDto.EmployeeId;
        record.Date = attendanceDto.Date;
        record.Status = attendanceDto.Status;

        unitOfWork.Attendances.Update(record);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<AttendanceDto>> GetAllAttendancesAsync(int? employeeId, int? deptId, DateTime? fromDate, DateTime? toDate)
    {
        var attendancesQuery =  unitOfWork.Attendances.GetAllQueryable();
        IQueryable<Employee> employeesQuery =  unitOfWork.Employees.GetAllQueryable().Include(e=>e.Department);
        var departmentsQuery = unitOfWork.Departments.GetAllQueryable();
        if (employeeId.HasValue)
        {
            attendancesQuery = attendancesQuery.Where(e => e.EmployeeId == employeeId.Value);
            employeesQuery = employeesQuery.Where(e => e.Code == employeeId.Value);
            departmentsQuery = departmentsQuery.Where(d => d.Employees.Any(emp => emp.Code == employeeId.Value));
        }
        if(deptId.HasValue)
        {
            employeesQuery = employeesQuery
           .Where(e => e.DepartmentId == deptId.Value);
        }
        if (fromDate.HasValue)
        {
            attendancesQuery = attendancesQuery.Where(a => a.Date >= fromDate.Value);
        }
        if (toDate.HasValue)
        {
            attendancesQuery = attendancesQuery.Where(a => a.Date <= toDate.Value);
        }
        var joinedDate = from att in attendancesQuery
                         join emp in employeesQuery on att.EmployeeId equals emp.Code
                         join dept in departmentsQuery on emp.DepartmentId equals dept.Id
                         where emp.DepartmentId == dept.Id
                         select new AttendanceDto
                         {
                             Id = att.Id,
                             EmployeeId = emp.Code,
                             FullName = $"{emp.FullName}",
                             DepartmentName = emp.Department.Name,
                             Date = att.Date,
                             Status = att.Status
                         };
        return await joinedDate.OrderByDescending(j => j.Date).ToListAsync();
    }

    public async Task<AttendanceDto> GetAttendanceAsync(int id)
    {
        var record = await unitOfWork.Attendances.GetByIdAsync(id);
        if (record == null)
            throw new Exception("Attendance record not found.");
        var emp = await unitOfWork.Employees.GetByIdWithIncludesAsync(e=>e.Code==record.EmployeeId,e=>e.Department);
        if (emp == null)
            throw new Exception("employee record not found.");
        return new AttendanceDto
        {
            Id = record.Id,
            EmployeeId = record.EmployeeId,
            FullName = $"{emp.FullName}",
            DepartmentName = emp.Department.Name,
            Date = record.Date,
            Status = record.Status
        };
    }
}
