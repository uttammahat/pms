using ProjectManagement.Common;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class ProjectMilestoneController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        // GET: ProjectMilestone
        public ActionResult AddProjectMilestone(int id)
        {
            ViewData["projects"] = context.Projects.Where(p => p.Id == id && p.Status == true).SingleOrDefault();
            var dependencies = context.ProjectMilestones.Where(p => p.ProjectId == id).ToList();
            ViewData["dependencies"] = dependencies;
            var employees = context.Employees.ToList();
            return View(employees);
        }

        public ActionResult SaveProjectMilestone(FormCollection fc)
        {
            var projectMilestoneObj = new ProjectMilestone
            {
                TaskDetail = fc["TaskDetail"],
                NumberOfDay = Convert.ToInt32(fc["NumberOfDay"]),
                ImpactedFunctionalities = fc["ImpactedFunctionalities"],
                Objective = fc["Objective"],
                TaskDeliverables = fc["TaskDeliverables"],
                Assumption = fc["Assumption"],
                Constraints = fc["Constraints"],
                EstimatedStartDate = Convert.ToDateTime(fc["EstimatedStartDate"]).Date,
                EstimatedCompletionDate = Convert.ToDateTime(fc["EstimatedCompletionDate"]).Date,
                ActualStartDate = Convert.ToDateTime(fc["ActualStartDate"]).Date,
                ActualCompletionDate = Convert.ToDateTime(fc["ActualCompletionDate"]).Date,
                PercentageCompletion = Convert.ToDouble(fc["PercentageCompletion"]),
                Status = fc["Status"],
                EmployeeId = Convert.ToInt32(fc["EmployeeId"]),
                ProjectId = Convert.ToInt32(fc["ProjectId"]),
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name
            };

            try
            {
                context.ProjectMilestones.Add(projectMilestoneObj);
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                EntityValidation.EntityValidationError(e);
            }

            string dependencies = fc["Dependencies"];

            if (!string.IsNullOrEmpty(dependencies))
            {
                var taskDependenciesObj = new TaskDependency
                {
                    TaskId = projectMilestoneObj.Id,
                    DependentTaskId = Convert.ToInt32(fc["Dependencies"])
                };

                context.TaskDependencies.Add(taskDependenciesObj);
                context.SaveChanges();
            }

            return RedirectToAction("AddProjectMilestone", new { id = projectMilestoneObj.ProjectId });
        }

        public ActionResult UpdateProjectMilestone(int id)
        {
            ViewData["employees"] = context.Employees.ToList();
            var dependencies = context.ProjectMilestones.Where(p => p.ProjectId == id).ToList();
            ViewData["dependencies"] = dependencies;
            var projectMilestones = context.ProjectMilestones.Find(id);
            return View(projectMilestones);
        }

        public ActionResult EditProjectMilestone(FormCollection fc)
        {
            var projectMilestoneObj = context.ProjectMilestones.Find(Convert.ToInt32(fc["Id"]));
            projectMilestoneObj.Id = Convert.ToInt32(fc["Id"]);
            projectMilestoneObj.TaskDetail = fc["TaskDetail"];
            projectMilestoneObj.NumberOfDay = Convert.ToInt32(fc["NumberOfDay"]);
            projectMilestoneObj.ImpactedFunctionalities = fc["ImpactedFunctionalities"];
            projectMilestoneObj.Objective = fc["Objective"];
            projectMilestoneObj.TaskDeliverables = fc["TaskDeliverables"];
            projectMilestoneObj.Assumption = fc["Assumption"];
            projectMilestoneObj.Constraints = fc["Constraints"];
            projectMilestoneObj.EstimatedStartDate = Convert.ToDateTime(fc["EstimatedStartDate"]).Date;
            projectMilestoneObj.EstimatedCompletionDate = Convert.ToDateTime(fc["EstimatedCompletionDate"]).Date;
            projectMilestoneObj.ActualStartDate = Convert.ToDateTime(fc["ActualStartDate"]).Date;
            projectMilestoneObj.ActualCompletionDate = Convert.ToDateTime(fc["ActualCompletionDate"]).Date;
            projectMilestoneObj.PercentageCompletion = Convert.ToDouble(fc["PercentageCompletion"]);
            projectMilestoneObj.Status = fc["Status"];
            projectMilestoneObj.EmployeeId = Convert.ToInt32(fc["EmployeeId"]);
            projectMilestoneObj.ProjectId = Convert.ToInt32(fc["ProjectId"]);
            projectMilestoneObj.ModifiedDate = DateTime.Now;
            projectMilestoneObj.ModifiedBy = User.Identity.Name;

            try
            {
                context.Entry(projectMilestoneObj).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                EntityValidation.EntityValidationError(e);
            }

            string dependencies = fc["Dependencies"];

            if (!string.IsNullOrEmpty(dependencies))
            {
                var taskDependenciesObj = new TaskDependency
                {
                    TaskId = projectMilestoneObj.Id,
                    DependentTaskId = Convert.ToInt32(fc["Dependencies"])
                };

                context.TaskDependencies.Add(taskDependenciesObj);
                context.SaveChanges();
            }

            return RedirectToAction("AddProjectMilestone", new { id = projectMilestoneObj.ProjectId });
        }

        public ActionResult DeleteProjectMilestone(int id)
        {
            var projectMilestone = context.ProjectMilestones.Find(id);
            //projectMilestone.Status = false;
            context.ProjectMilestones.Remove(projectMilestone);
            context.SaveChanges();
            return RedirectToAction("AddProjectMilestone", new { id = projectMilestone.ProjectId });
        }
    }
}