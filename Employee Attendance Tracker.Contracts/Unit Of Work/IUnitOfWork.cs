using Data_Layer.Models;
using Employee_Attendance_Tracker.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Attendance_Tracker.Contracts.Unit_Of_Work
{
    internal interface IUnitOfWork:IDisposable
    {
        IGenericRepo<Department> Departments { get; }
        IGenericRepo<Employee> Employees { get; }
        IGenericRepo<Attendance> Attendances { get; }
    }
}
