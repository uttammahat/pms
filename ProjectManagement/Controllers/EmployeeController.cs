using ProjectManagement.Common;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddEmployee()
        {
            var departments = context.Departments.ToList();
            var employees = context.Employees.Where(p => p.Status == true).ToList();

            ViewData["departments"] = departments;
            string mess = TempData["errMess"] as string;

            if (mess != null)
            {
                ViewData["errMess"] = mess;
            }

            return View(employees);
        }

        public async Task<ActionResult> SaveEmployee(Employee employee)
        {
            string root = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");

            var employeeObj = new Employee
            {
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Email = employee.Email,
                Contact = employee.Contact,
                JoinDate = employee.JoinDate,
                LeaveDate = employee.LeaveDate == null ? null : employee.LeaveDate,
                DateOfBorth = employee.DateOfBorth,
                DepartmentId = employee.DepartmentId,
                Username = employee.Username,
                Type = "User",
                Password = HashPassword.Encode(employee.Password),
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Status = true
            };

            context.Employees.Add(employeeObj);
            context.SaveChanges();

            string messBody = "Thank you <b>" + employeeObj.FirstName + " " + employeeObj.LastName + "</b> for joining us. Please click <a href='" + root + "/Account/AccountActivation/" + employeeObj.Id + "'>here</a> to activate your account.";

            await MailSender.SendEmail("Active your account", messBody, employeeObj.Email);
            TempData["errMess"] = "We have successfully send you an account activation link. Please verify that was you.";
            return RedirectToAction("AddEmployee");
        }

        public ActionResult GetAllEmployee()
        {
            return RedirectToAction("AddEmployee");
        }

        public ActionResult DeleteEmployee(int id)
        {
            var employee = context.Employees.Where(p => p.Id == id && p.Status == true).FirstOrDefault();
            employee.Status = false;
            context.Entry(employee).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AddEmployee");
        }

        public ActionResult EmployeeUpdate(int id)
        {
            var employee = context.Employees.Find(id);
            var departments = context.Departments.ToList();
            ViewData["departments"] = departments;

            return View(employee);
        }

        public ActionResult UpdateEmployee(Employee employee)
        {
            var employeeObj = context.Employees.Find(employee.Id);
            employeeObj.Id = employee.Id;
            employeeObj.FirstName = employee.FirstName;
            employeeObj.MiddleName = employee.MiddleName;
            employeeObj.LastName = employee.LastName;
            employeeObj.Email = employee.Email;
            employeeObj.Contact = employee.Contact;
            employeeObj.JoinDate = employee.JoinDate;
            employeeObj.LeaveDate = employee.LeaveDate;
            employeeObj.DateOfBorth = employee.DateOfBorth;
            employeeObj.DepartmentId = employee.DepartmentId;
            employeeObj.ModifiedDate = DateTime.Now;
            employeeObj.ModifiedBy = User.Identity.Name;

            context.Entry(employeeObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AddEmployee");
        }
    }
}