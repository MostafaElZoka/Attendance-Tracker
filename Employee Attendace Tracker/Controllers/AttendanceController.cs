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
            ViewBag.Employees = new SelectList(emps, "Code", "FullName.LastName");

            return View();
        }
        // GET: AttendanceController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult Delete(int id)
        {
            return View();
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
