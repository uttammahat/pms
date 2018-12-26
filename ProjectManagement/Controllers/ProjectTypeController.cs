using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class ProjectTypeController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddProjectType()
        {
            var ProjectTypes = context.ProjectTypes.ToList();
            return View(ProjectTypes);
        }

        public ActionResult SaveProjectType(ProjectType projectType)
        {
            context.ProjectTypes.Add(projectType);
            context.SaveChanges();

            return RedirectToAction("AddProjectType");
        }

        public ActionResult GetAllProjectType()
        {
            return RedirectToAction("AddProjectType");
        }

        public ActionResult DeleteProjectType(int id)
        {
            var projectTypes = context.ProjectTypes.Find(id);
            context.ProjectTypes.Remove(projectTypes);
            context.SaveChanges();
            
            return RedirectToAction("AddProjectType");
        }

        public ActionResult ProjectTypeUpdate(int id)
        {
            var ProjectType = context.ProjectTypes.Find(id);
            return View(ProjectType);
        }

        public ActionResult UpdateProjectType(ProjectType projectType)
        {
            var projectTypeObj = new ProjectType
            {
                Id = projectType.Id,
                Type = projectType.Type
            };

            context.Entry(projectTypeObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            
            return RedirectToAction("AddProjectType");
        }
    }
}