using Business_Layer.DTOs;
using Data_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Interfaces
{
    public interface IDepartmentService
    {
        public Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        public Task<Department> GetDepartmentByIdAsync(int id);
        public Task AddDepartment(DepartmentDto department);
        public Task UpdateDepartmentAsync(UpdateDepartmentDto department);
        public Task DeleteDepartmentAsync(int id);
    }
}
