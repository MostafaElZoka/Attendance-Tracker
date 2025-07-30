using Business_Layer.Extensions;
using Business_Layer.Interfaces;
using Data_Layer.Context;
using Data_Layer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Employee_Attendace_Tracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAttendanceDbContext();//injecting dbcontext

            builder.Services.AddBusinessServices(); //injecting services

            var app = builder.Build();
            //data seeding
            //using(var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<AttendanceDbContext>();
            //    context.SeedTestData();
            //}
            app.Services.Seed();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Attendance}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
