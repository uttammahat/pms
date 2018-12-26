using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddProject()
        {
            var model = context.Projects.Where(p => p.Status == true).ToList();
            ViewData["projectCategory"] = context.ProjectCategories.ToList();
            ViewData["projectType"]  = context.ProjectTypes.ToList();

            return View(model);
        }

        public ActionResult SaveProject(Project project)
        {
            var projectObj = new Project
            {
                Id = project.Id,
                ProjectTypeId = project.ProjectTypeId,
                ApprovalDate = project.ApprovalDate,
                Title = project.Title,
                ProjectCategoryId = project.ProjectCategoryId,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Introduction = project.Introduction,
                KeyDetails = project.KeyDetails,
                Scope = project.Scope,
                ProjectStatusId = 2,
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Status = true
            };

            context.Projects.Add(projectObj);
            context.SaveChanges();

            if (context.ProjectTypes.Where(p => p.Id == project.ProjectTypeId).SingleOrDefault().Type == "Inhouse")
            {
                return RedirectToAction("AddTeamLead", "TeamLead", new { id = projectObj.Id });
            }
            else
            {
                return RedirectToAction("AddClient", "Client", new { id = projectObj.Id });
            }
        }
        
        public ActionResult DeleteProject(int id)
        {
            var projects = context.Projects.Find(id);
            projects.Status = false;
            context.Entry(projects).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AddProject");
        }

        public ActionResult ProjectUpdate(int id)
        {
            var model = context.Projects.Where(p => p.Id == id && p.Status == true).FirstOrDefault();
            ViewData["projectCategory"] = context.ProjectCategories.ToList();
            ViewData["projectType"] = context.ProjectTypes.ToList();

            return View(model);
        }

        public ActionResult UpdateProject(Project project)
        {
            var projectObj = context.Projects.Find(project.Id);
            projectObj.Id = project.Id;
            projectObj.ProjectTypeId = project.ProjectTypeId;
            projectObj.ApprovalDate = project.ApprovalDate;
            projectObj.Title = project.Title;
            projectObj.ProjectCategoryId = project.ProjectCategoryId;
            projectObj.StartDate = project.StartDate;
            projectObj.EndDate = project.EndDate;
            projectObj.Introduction = project.Introduction;
            projectObj.KeyDetails = project.KeyDetails;
            projectObj.Scope = project.Scope;
            projectObj.ProjectStatusId = project.ProjectStatu.Id;
            projectObj.ModifiedDate = DateTime.Now;
            projectObj.ModifiedBy = User.Identity.Name;
            
            context.Entry(projectObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AddProject");
        }

        public ActionResult ViewProjectDetails(int id)
        {
            TempData["ProjectId"] = id;
            Project projectDetails = context.Projects.Where(p => p.Id == id && p.Status == true).SingleOrDefault();
            double dev = (DateTime.Now - DateTime.Parse(projectDetails.EndDate)).TotalDays;
            TimeSpan totalDays = DateTime.Parse(projectDetails.EndDate) - projectDetails.StartDate;

            double res = (dev / totalDays.TotalDays) * 100;
            ViewData["projectDeviation"] = res;
            return View(projectDetails);
        }

        public ActionResult StartProject()
        {
            int status = context.ProjectStatus.Where(a => a.Status == "Started").Select(p => p.Id).SingleOrDefault();
            var project = new Project { Id = Convert.ToInt32(TempData["ProjectId"]), ProjectStatusId = status };
            
            Project proj = context.Projects.Where(x => x.Id == project.Id && x.Status == true).FirstOrDefault();
            proj.ProjectStatusId = status;
            context.Entry(proj);
            context.SaveChanges();

            return RedirectToAction("ViewProjectDetails", new { id = TempData["ProjectId"] });
        }

        public ActionResult CompleteProject()
        {
            int status = context.ProjectStatus.Where(a => a.Status == "Completed").Select(p => p.Id).SingleOrDefault();
            var project = new Project { Id = Convert.ToInt32(TempData["ProjectId"]), ProjectStatusId = status };

            Project proj = context.Projects.Where(x => x.Id == project.Id && x.Status == true).FirstOrDefault();
            proj.ProjectStatusId = status;
            context.Entry(proj);
            context.SaveChanges();

            return RedirectToAction("ViewProjectDetails", new { id = TempData["ProjectId"] });
        }
    }
}