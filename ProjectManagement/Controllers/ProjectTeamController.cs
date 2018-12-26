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
    public class ProjectTeamController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();

        public ActionResult AddProjectTeam(FormCollection formCollection)
        {
            string[] keys = formCollection.AllKeys;
            int roleCount = 0;
            int resCount = 0;

            foreach (var item in keys)
            {
                if (item.Contains("Role"))
                {
                    roleCount = Convert.ToInt32((int)Char.GetNumericValue(item[item.Length - 1])) + 1;
                }

                if (item.Contains("Responsibilities"))
                {
                    resCount = Convert.ToInt32((int)Char.GetNumericValue(item[item.Length - 1])) + 1;
                }
            }

            var projectTeamObj = new ProjectTeam
            {
                ProjectId = Convert.ToInt32(formCollection["ProjectId"]),
                EmployeeId = Convert.ToInt32(formCollection["EmployeeId"]),
                Status = true
            };

            context.ProjectTeams.Add(projectTeamObj);
            context.SaveChanges();

            for (int i = 0; i < roleCount; i++)
            {
                var projectTeamRolesObj = new ProjectTeamRole
                {
                    Role = formCollection["Role" + i],
                    ProjectId = Convert.ToInt32(formCollection["ProjectId"]),
                    EmployeeId = Convert.ToInt32(formCollection["EmployeeId"]),
                    CreatedDate = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    Status = true
                };

                context.ProjectTeamRoles.Add(projectTeamRolesObj);
                context.SaveChanges();
            }

            for (int i = 0; i < resCount; i++)
            {
                var projectTeamResponsibilitesObj = new ProjectTeamResponsibilite
                {
                    Responsibilities = formCollection["Responsibilities" + i],
                    ProjectId = Convert.ToInt32(formCollection["ProjectId"]),
                    EmployeeId = Convert.ToInt32(formCollection["EmployeeId"]),
                    CreatedDate = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    Status = true
                };

                context.ProjectTeamResponsibilites.Add(projectTeamResponsibilitesObj);
                context.SaveChanges();
            }

            //TempData["projectId"] = formCollection["ProjectId"];
            return RedirectToAction("AddProjectMilestone", "ProjectMilestone", new { id = Convert.ToInt32(formCollection["ProjectId"]) });
        }
    }
}