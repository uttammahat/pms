using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class TeamResponsibilitiesController : Controller
    {
        ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddTeamResponsibilities(int id)
        {
            var employees = context.Employees.Where(p => p.Status == true).ToList();
            ViewData["projectId"] = id;
            var proposedTechnology = context.TechnologyPlatforms.Where(p => p.ProjectId == id && p.Status == true).SingleOrDefault();
            ViewData["proposedTechnology"] = proposedTechnology;
            ViewData["projectTeam"] = context.ProjectTeams.Where(p => p.ProjectId == id && p.Status == true).Where(q => q.Status == true).ToList();
            return View(employees);
        }

        public ActionResult DeleteEmployee(int empId, int projectId)
        {
            var projectTeam = context.ProjectTeams.Where(p => p.EmployeeId == empId && p.ProjectId == projectId && p.Status == true).SingleOrDefault();
            projectTeam.Status = false;
            context.Entry(projectTeam).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AddTeamResponsibilities", "TeamResponsibilities", new { id = projectId });
        }

        public ActionResult AddTechnologyPlatform(TechnologyPlatform technologyPlatform)
        {
            var technologyPlatformObj = new TechnologyPlatform
            {
                BackendTechnology = technologyPlatform.BackendTechnology,
                FrontendTechnology = technologyPlatform.FrontendTechnology,
                ReasonOfUse = technologyPlatform.ReasonOfUse,
                ProjectId = technologyPlatform.ProjectId,
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Status = true
            };

            context.TechnologyPlatforms.Add(technologyPlatformObj);
            context.SaveChangesAsync();

            return RedirectToAction("AddTeamResponsibilities", "TeamResponsibilities", new { id = technologyPlatform.ProjectId });
        }

        public ActionResult UpdateTeamResponsibilities(int empId, int projectId)
        {
            var employees = context.Employees.Where(p => p.Status == true).ToList();
            var employeeRoles = context.ProjectTeamRoles.Where(p => p.EmployeeId == empId && p.ProjectId == projectId && p.Status == true).ToList();
            var employeeResponsibilites = context.ProjectTeamResponsibilites.Where(p => p.EmployeeId == empId && p.ProjectId == projectId && p.Status == true).ToList();
            ViewData["employeeRoles"] = employeeRoles;
            ViewData["employeeResponsibilites"] = employeeResponsibilites;
            return View(employees);
        }

        public ActionResult UpdateRoles(ProjectTeamRole projectTeamRole)
        {
            var projectTeamRoleObj = context.ProjectTeamRoles.Find(projectTeamRole.Id);
            projectTeamRoleObj.Id = projectTeamRole.Id;
            projectTeamRoleObj.Role = projectTeamRole.Role;
            projectTeamRoleObj.ProjectId = projectTeamRole.ProjectId;
            projectTeamRoleObj.EmployeeId = projectTeamRole.EmployeeId;
            projectTeamRoleObj.ModifiedDate = DateTime.Now;
            projectTeamRoleObj.ModifiedBy = User.Identity.Name;

            context.Entry(projectTeamRoleObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("UpdateTeamResponsibilities", new { empId = projectTeamRole.EmployeeId, projectId = projectTeamRole.ProjectId });
        }

        public ActionResult UpdateResponsibilites(ProjectTeamResponsibilite projectTeamResponsibilite)
        {
            var projectTeamResponsibiliteObj = context.ProjectTeamResponsibilites.Find(projectTeamResponsibilite.Id);
            projectTeamResponsibiliteObj.Id = projectTeamResponsibilite.Id;
            projectTeamResponsibiliteObj.Responsibilities = projectTeamResponsibilite.Responsibilities;
            projectTeamResponsibiliteObj.ProjectId = projectTeamResponsibilite.ProjectId;
            projectTeamResponsibiliteObj.EmployeeId = projectTeamResponsibilite.EmployeeId;
            projectTeamResponsibiliteObj.ModifiedDate = DateTime.Now;
            projectTeamResponsibiliteObj.ModifiedBy = User.Identity.Name;

            context.Entry(projectTeamResponsibilite).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("UpdateTeamResponsibilities", new { empId = projectTeamResponsibilite.EmployeeId, projectId = projectTeamResponsibilite.ProjectId });
        }

        public ActionResult UpdateTechnologyPlatform(int id)
        {
            var technologyPlatForm = context.TechnologyPlatforms.Where(p => p.ProjectId == id).SingleOrDefault();
            return View(technologyPlatForm);
        }

        public ActionResult EditTechnologyPlatform(TechnologyPlatform technologyPlatform)
        {
            var technologyPlatformObj = context.TechnologyPlatforms.Find(technologyPlatform.Id);
            technologyPlatformObj.Id = technologyPlatform.Id;
            technologyPlatformObj.BackendTechnology = technologyPlatform.BackendTechnology;
            technologyPlatformObj.FrontendTechnology = technologyPlatform.FrontendTechnology;
            technologyPlatformObj.ReasonOfUse = technologyPlatform.ReasonOfUse;
            technologyPlatformObj.ProjectId = technologyPlatform.ProjectId;
            technologyPlatformObj.ModifiedDate = DateTime.Now;
            technologyPlatformObj.ModifiedBy = User.Identity.Name;

            context.Entry(technologyPlatformObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("AddTeamResponsibilities", "TeamResponsibilities", new { id = technologyPlatform.ProjectId });
        }
    }
}