using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Employee_Attendace_Tracker.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList;
using X.PagedList.Extensions;
namespace Employee_Attendace_Tracker.Controllers
{
    public class EmployeeController(IEmployeeService employeeService,IDepartmentService departmentService) : Controller
    {

        // GET: EmployeeController1
        public async Task<ActionResult> Index(int? page)
        {
            int pageSize = 2;
            int pageNumber = page ?? 1;
            var emps = await employeeService.GetAllEmployeesAsync();
            var depts = await departmentService.GetAllDepartmentsAsync();

            ViewBag.Departments = new SelectList(depts, "Id", "Name");
            return View(emps.ToPagedList(pageNumber,pageSize));
        }

        // GET: EmployeeController1/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var emp = await employeeService.GetEmployeeDtoByIdAsync(id);
                var summary = await employeeService.GetAttendanceMonthSummaryAsync(id);
                var dept = await departmentService.GetDepartmentDtoByIdAsync(emp.DepartmentId);

                var viewModel = new EmployeeDetailsViewModel
                {
                    Employee = emp,
                    AttendanceSummary = summary,
                    DepartmentName = dept.Name
                };

                return View(viewModel);

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View(id);
            }

        }

        // GET: EmployeeController1/Create
        public async Task<ActionResult> Create()
        {
            var depts = await departmentService.GetAllDepartmentsAsync();

            ViewBag.Departments = new SelectList(depts, "Id", "Name");
            return View();
        }

        // POST: EmployeeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AddEmployeeDto employeeDto)
        {
            if(!ModelState.IsValid)
                return View(employeeDto);
            try
            {
                await employeeService.AddEmployee(employeeDto);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(employeeDto);
            }
        }

        // GET: EmployeeController1/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var emp = await employeeService.GetEmployeeDtoByIdAsync(id);
                var depts = await departmentService.GetAllDepartmentsAsync();

                ViewBag.Departments = new SelectList(depts, "Id", "Name");
                return View(emp);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading employee: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: EmployeeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return View(employeeDto);

            try
            {
                await employeeService.UpdateEmployeeAsync(employeeDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var depts = await departmentService.GetAllDepartmentsAsync();

                ViewBag.Departments = new SelectList(depts, "Id", "Name");
                ModelState.AddModelError("", ex.Message);
                return View(employeeDto);
            }
        }

        // GET: EmployeeController1/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var emp = await employeeService.GetEmployeeDtoByIdAsync(id);
                var dept = await departmentService.GetDepartmentDtoByIdAsync(emp.DepartmentId);
                ViewBag.Department = dept?.Name ?? "Unknown";
                ViewBag.DepartmentId = emp.DepartmentId;
                return View(emp);
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: EmployeeController1/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await employeeService.DeleteEmployeeAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting Employee: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}
