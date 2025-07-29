
using Data_Layer.Context;
using Data_Layer.Models;
using Data_Layer.Repository;

namespace Data_Layer.Unit_Of_Work;

internal class UnitOfWork : IUnitOfWork
{
    private readonly AttendanceDbContext _context;
    public IGenericRepo<Department> Departments { get; }

    public IGenericRepo<Employee> Employees { get; }

    public IGenericRepo<Attendance> Attendances { get; }

    public UnitOfWork(AttendanceDbContext context)
    {
        _context = context;
        Departments = new GenericRepo<Department>(_context);
        Employees = new GenericRepo<Employee>(_context);
        Attendances = new GenericRepo<Attendance>(_context);
    }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
