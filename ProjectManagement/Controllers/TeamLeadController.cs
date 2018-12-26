using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class TeamLeadController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        
        public ActionResult AddTeamLead(int id)
        {
            var employeeData = context.Employees.ToList();
            ViewData["projectData"] = context.Projects.Where(p => p.Id == id).SingleOrDefault();
            return View(employeeData);
        }

        public ActionResult SaveTeamLead(TeamLead teamLead)
        {
            var teamLeadObj = new TeamLead
            {
                ProjectId = teamLead.ProjectId,
                EmployeeId = teamLead.EmployeeId,
                StartDate = teamLead.StartDate,
                EndDate = null,
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Status = true
            };

            context.TeamLeads.Add(teamLeadObj);
            context.SaveChanges();

            return RedirectToAction("AddTeamResponsibilities", "TeamResponsibilities", new { id = teamLeadObj.ProjectId });
        }
    }
}