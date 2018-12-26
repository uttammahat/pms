using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        //GET: Home
        public ActionResult Index()
        {
            int started = context.Projects.Count(p => p.ProjectStatu.Status == "Started");
            int notStarted = context.Projects.Count(p => p.ProjectStatu.Status == "NotStarted");
            int completed = context.Projects.Count(p => p.ProjectStatu.Status == "Completed");

            List<DataPoint> dataPoints = new List<DataPoint>();

            dataPoints.Add(new DataPoint("Albert", 10));
            dataPoints.Add(new DataPoint("Tim", 30));
            dataPoints.Add(new DataPoint("Wilson", 17));
            dataPoints.Add(new DataPoint("Joseph", 39));
            dataPoints.Add(new DataPoint("Robert", 30));
            dataPoints.Add(new DataPoint("Sophia", 25));
            dataPoints.Add(new DataPoint("Emma", 15));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);


            //project status
            List<DataPoint> projectStatus = new List<DataPoint>();

            decimal total = started + notStarted + completed;
            decimal startedPercentage = started / total * 100;
            decimal notStartedPercentage = notStarted / total * 100;
            decimal completedPercentage = completed / total * 100;

            projectStatus.Add(new DataPoint("Started", startedPercentage));
            projectStatus.Add(new DataPoint("Not Started", notStartedPercentage));
            projectStatus.Add(new DataPoint("Completed", completedPercentage));

            ViewBag.ProjectStatus = JsonConvert.SerializeObject(projectStatus);

            var projectList = context.Projects.ToList();
            return View(projectList);
        }
    }
}