using Business_Layer.DTOs;
using Business_Layer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Employee_Attendace_Tracker.Controllers
{
    public class AttendanceController(IAttendanceService attendanceService,IEmployeeService employeeService, 
        IDepartmentService departmentService) : Controller
    {
        // GET: AttendanceController
        public async Task<IActionResult> Index(int? employeeId, int? deptId, DateTime? fromDate, DateTime? toDate)
        {
            var attendances = await attendanceService.GetAllAttendancesAsync(employeeId,deptId,fromDate,toDate);
            var emps = await employeeService.GetAllEmployeesAsync();
            var depts = await departmentService.GetAllDepartmentsAsync();

            ViewBag.Employees = new SelectList(emps, "Code", "DisplayFullName");
            ViewBag.Departments = new SelectList(depts, "Id", "Name");
            return View(attendances);
        }

        public async Task<IActionResult> Mark()
        {
            var emps = await employeeService.GetAllEmployeesAsync();
            ViewBag.Employees = new SelectList(emps, "Code", "DisplayFullName");

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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttendanceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: AttendanceController/Delete/5
        public async Task<ActionResult> Delete(int employeeId, DateTime date)
        {
            var record = (await attendanceService.GetAllAttendancesAsync(employeeId, null, date, date)).FirstOrDefault();

            return View(record);
        }

        // POST: AttendanceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
