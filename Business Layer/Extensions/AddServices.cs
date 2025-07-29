using Business_Layer.Interfaces;
using Business_Layer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Business_Layer.Extensions
{
    public static class AddServices
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            // Add your service registrations here
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAttendanceService, AttendanceService>();

            // Add other services as needed
        }
    }
}
