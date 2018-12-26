using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddDepartment()
        {
            var model = context.Departments.ToList();
            return View(model);
        }

        public ActionResult SaveDepartment(Department department)
        {
            var departmentObj = new Department
            {
                Name = department.Name
            };

            context.Departments.Add(departmentObj);
            context.SaveChanges();
            return RedirectToAction("AddDepartment");
        }

        public ActionResult GetAllDepartment()
        {
            return RedirectToAction("AddDepartment");
        }

        public ActionResult DeleteDepartment(int id)
        {
            var departments = context.Departments.Find(id);
            context.Departments.Remove(departments);
            context.SaveChanges();

            return RedirectToAction("AddDepartment");
        }

        public ActionResult DepartmentUpdate(int id)
        {
            var model = context.Departments.Find(id);
            return View(model);
        }

        public ActionResult UpdateDepartment(Department department)
        {
            var departmentObj = new Department
            {
                Id = department.Id,
                Name = department.Name
            };

            context.Entry(departmentObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AddDepartment");
        }
    }
}