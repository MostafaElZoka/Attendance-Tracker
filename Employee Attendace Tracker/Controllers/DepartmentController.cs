﻿using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Business_Layer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Employee_Attendace_Tracker.Controllers
{
    public class DepartmentController(IDepartmentService departmentService, IEmployeeService employeeService) : Controller
    {
        // GET: DepartmentController
        public async Task<IActionResult> Index()
        {
            var depts = await departmentService.GetAllDepartmentsAsync();
            return View(depts);
        }


        // GET: DepartmentController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DepartmentDto dto)
        {
            if(!ModelState.IsValid)
             return View(dto);

            try
            {
                await departmentService.AddDepartment(dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("",exception.Message);
                return View(dto);
            }

        }

        // GET: DepartmentController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            try
            {
                var dept = await departmentService.GetDepartmentForUpdateAsync(id);
;
                return View(dept);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading department: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
                return View(departmentDto);

            try
            {
                await departmentService.UpdateDepartmentAsync(departmentDto);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(departmentDto);
            }
        }

        // GET: DepartmentController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var department = await departmentService.GetDepartmentDtoByIdAsync(id);
                var employees = (await employeeService.GetAllEmployeesAsync())
                    .Where(e => e.DepartmentId == id);

                ViewBag.EmployeeCount = employees.Count();

                return View(department);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: DepartmentController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await departmentService.DeleteDepartmentAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting department: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }
    }
}
