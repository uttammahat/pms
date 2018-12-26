using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class RiskRegisterController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddRiskRegister(int id)
        {
            string projectName = context.Projects.Where(p => p.Id == id && p.Status == true).Select(p => p.Title).SingleOrDefault();
            ViewData["projectName"] = projectName;
            ViewData["projectId"] = id;

            var riskRegisters = context.RiskRegisters.Where(p => p.ProjectId == id && p.Status == true).ToList();
            return View(riskRegisters);
        }

        public ActionResult SaveRiskRegister(FormCollection formCollection)
        {

            var riskRegisterObj = new RiskRegister
            {
                CurrentStatus = formCollection["CurrentStatus"] == "Open" ? true : false,
                RiskImpact = formCollection["RiskImpact"],
                ProbabilityOfOccurrence = formCollection["ProbabilityOfOccurrence"],
                RiskDescription = formCollection["RiskDescription"],
                ProjectImpact = formCollection["ProjectImpact"],
                Symptoms = formCollection["Symptoms"],
                RiskResponseStrategy = formCollection["RiskResponseStrategy"],
                ResponseStrategy = formCollection["ResponseStrategy"],
                ContingencyPlan = formCollection["ContingencyPlan"],
                ProjectId = Convert.ToInt32(formCollection["ProjectId"]),
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Status = true
            };

            context.RiskRegisters.Add(riskRegisterObj);
            context.SaveChanges();

            string[] keys = formCollection.AllKeys;
            int riskRegisterCount = 0;

            foreach (var item in keys)
            {
                if (item.Contains("RiskArea"))
                {
                    riskRegisterCount = Convert.ToInt32((int)Char.GetNumericValue(item[item.Length - 1])) + 1;
                }
            }

            for (int i = 0; i < riskRegisterCount; i++)
            {
                var riskAreaObj = new RiskArea
                {
                    RiskAreas = formCollection["RiskArea" + i],
                    RiskRegisterId = riskRegisterObj.Id
                };

                context.RiskAreas.Add(riskAreaObj);
                context.SaveChanges();
            }

            return RedirectToAction("ViewProjectDetails", "Project", new { id = Convert.ToInt32(formCollection["ProjectId"]) });
        }

        public ActionResult EditRiskRegister(int id)
        {
            var risks = context.RiskRegisters.Where(p => p.Id == id).FirstOrDefault();
            return View(risks);
        }

        public ActionResult UpdateRiskRegister(RiskRegister riskRegister)
        {
            var riskRegisterObj = context.RiskRegisters.Find(riskRegister.Id);
            riskRegisterObj.Id = riskRegister.Id;
            riskRegisterObj.CurrentStatus = riskRegister.CurrentStatus;
            riskRegisterObj.RiskImpact = riskRegister.RiskImpact;
            riskRegisterObj.ProbabilityOfOccurrence = riskRegister.ProbabilityOfOccurrence;
            riskRegisterObj.RiskDescription = riskRegister.RiskDescription;
            riskRegisterObj.ProjectImpact = riskRegister.ProjectImpact;
            riskRegisterObj.Symptoms = riskRegister.Symptoms;
            riskRegisterObj.RiskResponseStrategy = riskRegister.RiskResponseStrategy;
            riskRegisterObj.ResponseStrategy = riskRegister.ResponseStrategy;
            riskRegisterObj.ContingencyPlan = riskRegister.ContingencyPlan;
            riskRegisterObj.ProjectId = riskRegister.ProjectId;
            riskRegisterObj.ModifiedDate = DateTime.Now;
            riskRegisterObj.ModifiedBy = User.Identity.Name;

            context.Entry(riskRegister).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("ViewProjectDetails", "Project", new { id = riskRegister.ProjectId });
        }

        public ActionResult EditRiskArea(int id)
        {
            ViewData["id"] = id;
            var areas = context.RiskAreas.Where(p => p.RiskRegisterId == id).ToList();
            return View(areas);
        }

        public ActionResult UpdateRiskArea(RiskArea riskArea)
        {
            var riskAreaObj = new RiskArea
            {
                Id = riskArea.Id,
                RiskAreas = riskArea.RiskAreas,
                RiskRegisterId = riskArea.RiskRegisterId
            };

            context.Entry(riskAreaObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("EditRiskArea", new { id = riskAreaObj.RiskRegisterId });
        }

        public ActionResult DeleteRiskRegister(int id)
        {
            var res = context.RiskRegisters.Find(id);
            res.Status = false;
            
            try
            {
                context.Entry(res).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return RedirectToAction("AddRiskRegister", new { id = res.ProjectId });
        }
    }
}