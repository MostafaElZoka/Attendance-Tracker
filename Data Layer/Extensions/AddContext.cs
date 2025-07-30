using Data_Layer.Context;
using Data_Layer.Unit_Of_Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Extensions
{
    public static class AddContext
    {
        public static void AddAttendanceDbContext(this IServiceCollection services)
        {
            services.AddDbContext<AttendanceDbContext>(options =>
                options.UseInMemoryDatabase("AttendanceDb"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
        public static void Seed(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AttendanceDbContext>();
                context.SeedTestData();
            }
        }

    }
}
