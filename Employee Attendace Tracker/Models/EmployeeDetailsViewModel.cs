using Business_Layer.DTOs;

namespace Employee_Attendace_Tracker.Models
{
    public class EmployeeDetailsViewModel
    {
        public EmployeeDto Employee { get; set; }
        public AttendanceSummaryDto AttendanceSummary { get; set; }
        public string DepartmentName { get; set; }
    }
}
