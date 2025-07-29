
using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Data_Layer.Models;
using Data_Layer.Unit_Of_Work;

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
            Date = attendanceDto.Date,
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

        if (!await unitOfWork.Employees.Exists(e => e.Code == attendanceDto.EmployeeId))
            throw new Exception("Employee does not exist.");

        if(await unitOfWork.Attendances.Exists(a=> a.EmployeeId == attendanceDto.EmployeeId && a.Date == attendanceDto.Date))
            throw new Exception("Attendance record for this employee on this date already exists.");

        record.EmployeeId = attendanceDto.EmployeeId;
        record.Date = attendanceDto.Date;
        record.Status = attendanceDto.Status;

        unitOfWork.Attendances.Update(record);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<AttendanceDto>> GetAllAttendancesAsync(int? employeeId, int? deptId, DateTime? fromDate, DateTime? toDate)
    {
        var attendances = await unitOfWork.Attendances.GetAllAsync();
        var employees = await unitOfWork.Employees.GetAllAsync(e=>e.Department);
        if(employeeId.HasValue)
        {
            employees = employees.Where(e => e.Code == employeeId.Value);
        }
        if(deptId.HasValue)
        {
            employees = employees.Where(e => e.DepartmentId == deptId.Value);
        }
        if (fromDate.HasValue)
        {
            attendances = attendances.Where(a => a.Date >= fromDate.Value);
        }
        if (toDate.HasValue)
        {
            attendances = attendances.Where(a => a.Date <= toDate.Value);
        }
        var joinedDate = from att in attendances
                         join emp in employees on att.EmployeeId equals emp.Code 
                         select new AttendanceDto
                         {
                             EmployeeId = emp.Code,
                             FullName = $"{emp.FullName.FirstName} {emp.FullName.SecondName} {emp.FullName.ThirdName} {emp.FullName.LastName}",
                             DepartmentName = emp.Department.Name,
                             Date = att.Date,
                             Status = att.Status
                         };
        return joinedDate.OrderByDescending(j => j.Date);
    }
}
