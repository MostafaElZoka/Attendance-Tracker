using Data_Layer.Context;
using Data_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Layer.Extensions
{
    public static class SeedExtension
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
                FullName = new FullName
                {
                    FirstName = "Ahmed",
                    SecondName = "Mohamed",
                    ThirdName = "Ali",
                    LastName = "Omar"
                },
                Email = "ahmed.omar@example.com",
                DepartmentId = hrDept.Id
            };

            var emp2 = new Employee
            {
                FullName = new FullName
                {
                    FirstName = "Amr",
                    SecondName = "Ibrahim",
                    ThirdName = "Ali",
                    LastName = "Salama"
                },
                Email = "amr.salama@example.com",
                DepartmentId = itDept.Id
            }; 

            context.Employees.AddRange(emp1, emp2);
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
                EmployeeId = emp1.Code,
                Date = today.AddDays(-3),
                Status = AttendanceStatus.Absent
            },

            new Attendance
            {
                EmployeeId = emp2.Code,
                Date = today.AddDays(-3),
                Status = AttendanceStatus.Absent
            });

            context.SaveChanges();
        }
    }
}
