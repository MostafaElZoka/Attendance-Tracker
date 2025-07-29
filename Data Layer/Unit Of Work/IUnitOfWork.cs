

using Data_Layer.Models;
using Data_Layer.Repository;

namespace Data_Layer.Unit_Of_Work;

public interface IUnitOfWork
{
    IGenericRepo<Department> Departments { get; }
    IGenericRepo<Employee> Employees { get; }
    IGenericRepo<Attendance> Attendances { get; }
    Task<int> SaveChangesAsync();
}
