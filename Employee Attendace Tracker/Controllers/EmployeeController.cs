using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Employee_Attendace_Tracker.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
namespace Employee_Attendace_Tracker.Controllers
{
    public class EmployeeController(IEmployeeService employeeService) : Controller
    {
        // GET: EmployeeController1
        public async Task<ActionResult> Index()
        {
            var emps = await employeeService.GetAllEmployeesAsync();

            return View(emps);
        }

        // GET: EmployeeController1/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var emp = await employeeService.GetEmployeeByIdAsync(id);
                var summary = await employeeService.GetAttendanceMonthSummaryAsync(id);

                var viewModel = new EmployeeDetailsViewModel
                {
                    Employee = emp,
                    AttendanceSummary = summary
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
        public ActionResult Create()
        {
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
                var emp = await employeeService.GetEmployeeByIdAsync(id);
                var updatedEmp = new EmployeeDto
                {
                    Code = emp.Code,
                    FullName = emp.FullName,
                    Email = emp.Email,
                    DepartmentId = emp.DepartmentId
                };
                return View(updatedEmp);
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
                ModelState.AddModelError("", ex.Message);
                return View(employeeDto);
            }
        }

        // GET: EmployeeController1/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var emp = await employeeService.GetEmployeeByIdAsync(id);
                return View(new EmployeeDto
                {
                    Code = emp.Code,
                    FullName = emp.FullName,
                    Email = emp.Email,
                    DepartmentId = emp.DepartmentId
                });
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
