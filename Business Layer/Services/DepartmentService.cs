using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Data_Layer.Models;
using Data_Layer.Repository;
using Data_Layer.Unit_Of_Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
    {
        public async Task AddDepartment(DepartmentDto departmentDto)
        {
            if (departmentDto.Code.Length != 4)
                throw new Exception("Code must be exactly 4 characters");
            
            if (await unitOfWork.Departments.Exists(d => d.Code == departmentDto.Code))
                throw new Exception("Department code must be unique");

            if (await unitOfWork.Departments.Exists(d => d.Name == departmentDto.Name))
                throw new Exception("Department name must be unique");

            var dept = new Department
            {
                Name = departmentDto.Name,
                Location = departmentDto.Location,
                Code = departmentDto.Code.ToUpper()
            };
           await unitOfWork.Departments.AddAsync(dept);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var dept = await unitOfWork.Departments.GetByIdAsync(id);
            if (dept == null)
            {
                throw new Exception($"Department with ID {id} not found.");
            }
            unitOfWork.Departments.Delete(dept);
             await unitOfWork.SaveChangesAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            var dept = await unitOfWork.Departments.GetByIdAsync(id);
            if (dept == null)
            {
                throw new Exception($"Department with ID {id} not found.");
            }
            return dept;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            var depts = await unitOfWork.Departments.GetAllAsync();
            return depts;
        }

        public async Task UpdateDepartmentAsync(UpdateDepartmentDto department)
        {
            var dept = await unitOfWork.Departments.GetByIdAsync(department.Id);
            if (dept == null)
            {
                throw new Exception($"Department with ID {department.Id} not found.");
            }

            if (department.Code.Length > 4)
                throw new Exception("Code must no exceed 4 characters");

            if (dept.Code != department.Code &&
                await unitOfWork.Departments.Exists(d => d.Code == department.Code))
                throw new Exception("Department code must be unique");

            if (dept.Name != department.Name && await unitOfWork.Departments.Exists(d => d.Name == department.Name))
                throw new Exception("Department name must be unique");

            dept.Name = department.Name;
            dept.Location = department.Location;
            dept.Code = department.Code.ToUpper();
            unitOfWork.Departments.Update(dept);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
