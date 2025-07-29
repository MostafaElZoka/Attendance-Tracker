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
        public Task AddDepartment(Department department)
        {
            throw new NotImplementedException();
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

        public Task UpdateDepartmentAsync(Department department)
        {
            throw new NotImplementedException();
        }
    }
}
