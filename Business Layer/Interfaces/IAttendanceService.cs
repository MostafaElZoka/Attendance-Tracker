
using Business_Layer.DTOs;

namespace Business_Layer.Interfaces;

public interface IAttendanceService
{
    public Task<IEnumerable<AttendanceDto>> GetAllAttendancesAsync(int? employeeId,int? deptId,DateTime? fromDate, DateTime? toDate,string filterMethod="");
    public Task EditAttendance(int id,UpdateOrAddAttendanceDto attendanceDto);
    public Task DeleteAttendance(int id);
    public Task AddAttendanceAsync(UpdateOrAddAttendanceDto attendanceDto);
    public Task<AttendanceDto> GetAttendanceAsync(int id);
}
