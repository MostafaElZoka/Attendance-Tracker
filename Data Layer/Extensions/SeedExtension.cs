using Data_Layer.Context;
using Data_Layer.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Extensions
{
    internal static class SeedExtension
    {
        public static void SeedTestData(this AttendanceDbContext context)
        {
            var hrDept = new Department
            {
                Code = "HRMG",
                Name = "Human Resources",
                Location = "Alexandria",

            };

            var itDept = new Department
            {
                Code = "ITMG",
                Name = "Information Technology",
                Location = "Cairo",
            };

            var financeDept = new Department
            {
                Code = "FIMG",
                Name = "Finance",
                Location = "Giza",
            };
            context.Departments.AddRange(hrDept, itDept, financeDept);
            context.SaveChanges();


            var emp1 = new Employee
            {
                FullName = "Youssef Mohamed Ali Omar",
                Email = "ahmed.omar@example.com",
                DepartmentId = hrDept.Id
            };

            var emp2 = new Employee
            {
                FullName = "Amr Magdy Mostafa Salama",
                Email = "amr.salama@example.com",
                DepartmentId = itDept.Id
            };

            var emp3 = new Employee
            {
                FullName = "Maria Sleem Youssef Reyad",
                Email = "maria.sleem@example.com",
                DepartmentId = hrDept.Id
            };

            context.Employees.AddRange(emp1, emp2, emp3);
            context.SaveChanges();

            var today = DateTime.Today;

            context.Attendances.AddRange(
            new Attendance
            {
                EmployeeId = emp2.Code,
                Date = today,
                Status = AttendanceStatus.Present
            },
            new Attendance
            {
                EmployeeId = emp1.Code,
                Date = today.AddDays(-2),
                Status = AttendanceStatus.Present
            },
            new Attendance
            {
                EmployeeId = emp3.Code,
                Date = today.AddDays(-1),
                Status = AttendanceStatus.Present
            },
            new Attendance
            {
                EmployeeId = emp3.Code,
                Date = today.AddDays(-3),
                Status = AttendanceStatus.Present
            },
            new Attendance
            {
                EmployeeId = emp1.Code,
                Date = today.AddDays(-1),
                Status = AttendanceStatus.Absent
            },

            new Attendance
            {
                EmployeeId = emp2.Code,
                Date = today.AddDays(-4),
                Status = AttendanceStatus.Absent
            });

            context.SaveChanges();
        }
    }
}
