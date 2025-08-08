using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Data_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace Employee_Attendace_Tracker.Controllers
{
    public class AttendanceController(IAttendanceService attendanceService,IEmployeeService employeeService, 
        IDepartmentService departmentService) : Controller
    {
        // GET: AttendanceController
        public async Task<IActionResult> Index(int? employeeId, int? deptId, DateTime? fromDate, DateTime? toDate,int? page)
        {
            var pageSize = 2;
            var pageNumber = page ?? 1;

            var attendances = await attendanceService.GetAllAttendancesAsync(employeeId,deptId,fromDate,toDate);
            var emps = await employeeService.GetAllEmployeesAsync();
            var depts = await departmentService.GetAllDepartmentsAsync();

            ViewBag.Employees = new SelectList(emps, "Code", "FullName",employeeId);
            ViewBag.Departments = new SelectList(depts, "Id", "Name",deptId);
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            return View(attendances.ToPagedList(pageNumber,pageSize));
        }

        public async Task<IActionResult> Mark()
        {
            var emps = await employeeService.GetAllEmployeesAsync();
            ViewBag.Employees = new SelectList(emps, "Code", "FullName");

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetStatus(int employeeId, DateTime date)
        {
            if(date > DateTime.Today)
            {
                return Json(new { status ="Can't edit future dates" });
            }
            var attendance = (await attendanceService.GetAllAttendancesAsync(employeeId, null, date, date)).FirstOrDefault();

            return Json(new {status = attendance?.Status.ToString()?? "Not Assigned" });

        }

        [HttpPost]
        public async Task<IActionResult> SaveAttendance([FromBody]UpdateOrAddAttendanceDto dto)
        {
            var attendance = (await attendanceService.GetAllAttendancesAsync(dto.EmployeeId, null, dto.Date, dto.Date))
                    .FirstOrDefault();
            if(attendance != null)
            {
                await attendanceService.EditAttendance(attendance.Id, dto);
            }
            else
            {
                await attendanceService.AddAttendanceAsync(dto);
            }
            return Json(new {success = true});
        }
        // GET: AttendanceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttendanceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendanceController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var record = await attendanceService.GetAttendanceAsync(id);
                var emp = await employeeService.GetEmployeeDtoByIdAsync(record.EmployeeId);
                var dept = await departmentService.GetDepartmentDtoByIdAsync(emp.DepartmentId);
                List<string> stats = Enum.GetNames(typeof(AttendanceStatus)).ToList();

                ViewBag.EmployeeName = emp.FullName;
                ViewBag.Statuses = stats;
                ViewBag.DepartmentName = dept.Name;
                return View(record);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading record: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: AttendanceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateOrAddAttendanceDto dto)
        {

            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            try
            {
                await attendanceService.EditAttendance(id, dto);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(dto);
            }
        }

        // GET: AttendanceController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var record = await attendanceService.GetAttendanceAsync(id);

            return View(record);
        }

        // POST: AttendanceController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await attendanceService.DeleteAttendance(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
