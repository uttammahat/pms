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
    public class ProjectDetailsController: Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddProjectDetails(int id)
        {
            ViewData["ProjectId"] = id;
            var model = context.ProjectDetails.Where(p => p.ProjectId == id).FirstOrDefault();
            return View(model);
        }

        public ActionResult SaveProjectDetails(ProjectDetail projectDetail)
        {
            var projectDetailObj = new ProjectDetail
            {
                CompletedDate = projectDetail.CompletedDate,
                KeyOutcomes = projectDetail.KeyOutcomes,
                MajorRoadblocks = projectDetail.MajorRoadblocks,
                KeyLearning = projectDetail.KeyLearning,
                ThingsCouldHaveBeenBetter = projectDetail.ThingsCouldHaveBeenBetter,
                RemrksFromTeamLead = projectDetail.RemrksFromTeamLead,
                RemarksFromDepartmentLead = projectDetail.RemarksFromDepartmentLead,
                RemarksFromManager = projectDetail.RemarksFromManager,
                ProjectId = projectDetail.ProjectId,
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name
            };

            try
            {
                context.ProjectDetails.Add(projectDetailObj);
                context.SaveChanges();
            } catch(DbEntityValidationException e)
            {
                EntityValidation.EntityValidationError(e);
                Console.WriteLine(e.Message);
            }

            return RedirectToAction("AddProjectDetails", new { id = projectDetailObj.ProjectId });
        }

        public ActionResult EditProjectDetails(int id)
        {
            var model = context.ProjectDetails.Find(id);
            return View(model);
        }

        public ActionResult UpdateProjectDetails(ProjectDetail projectDetail)
        {
            var projectDetailsObj = new ProjectDetail
            {
                Id = projectDetail.Id,
                CompletedDate = projectDetail.CompletedDate,
                KeyOutcomes = projectDetail.KeyOutcomes,
                MajorRoadblocks = projectDetail.MajorRoadblocks,
                KeyLearning = projectDetail.KeyLearning,
                ThingsCouldHaveBeenBetter = projectDetail.ThingsCouldHaveBeenBetter,
                RemrksFromTeamLead = projectDetail.RemrksFromTeamLead,
                RemarksFromDepartmentLead = projectDetail.RemarksFromDepartmentLead,
                RemarksFromManager = projectDetail.RemarksFromManager,
                ProjectId = projectDetail.ProjectId,
                ModifiedDate = DateTime.Now,
                ModifiedBy = User.Identity.Name
            };

            try
            {
                context.Entry(projectDetailsObj).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return RedirectToAction("AddProjectDetails", new { id = projectDetailsObj.ProjectId });
        }

        public ActionResult DeleteProjectDetails(int id)
        {
            try
            {
                var data = context.ProjectDetails.Find(id);
                context.ProjectDetails.Remove(data);
                context.SaveChanges();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RedirectToAction("AddProjectDetails", new { id = id });
        }
    }
}