using ProjectManagement.Common;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddClient(int id)
        {
            ViewData["isContractor"] = "yes";
            List<Client> model = context.Clients.Where(p => p.ProjectId == id && p.Status == true).ToList();
            ViewData["projects"] = context.Projects.Where(p => p.Id == id && p.Status == true).SingleOrDefault();
            return View(model);
        }

        public ActionResult SaveClient(Client client)
        {
            var ObjClient = new Client
            {
                CompanyName = client.CompanyName,
                ContactNo = client.ContactNo,
                Email = client.Email,
                Address = client.Address,
                Website = client.Website,
                ProjectId = client.ProjectId,
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Status = true
            };

            context.Clients.Add(ObjClient);
            context.SaveChanges();

            if (client.Contractor == "yes")
            {
                ViewData["isContractor"] = "yes";
                //return RedirectToAction("AddContract", "Contract", new { id = ObjClient.ProjectId });
            }
            else
            {
                ViewData["isContractor"] = "no";
                //return RedirectToAction("AddTeamLead", "TeamLead", new { id = ObjClient.ProjectId });
            }
            return RedirectToAction("AddClient", new { id = ObjClient.ProjectId });
        }

        public ActionResult GetAllClient()
        {
            return RedirectToAction("AddClient");
        }

        public ActionResult DeleteClient(int id)
        {
            var clients = context.Clients.Find(id);
            clients.Status = false;

            context.Entry(clients).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("ViewProjectDetails", "Project", new { id = clients.ProjectId });
        }

        public ActionResult ClientUpdate(int id, int projectId)
        {
            Client model = context.Clients.Where(p => p.Id == id && p.Status == true).FirstOrDefault();
            ViewData["projects"] = context.Projects.Where(p => p.Id == projectId && p.Status == true).ToList();
            return View(model);
        }

        public ActionResult UpdateClient(Client client)
        {
            var clientObj = context.Clients.Find(client.Id);
            clientObj.Id = client.Id;
            clientObj.CompanyName = client.CompanyName;
            clientObj.ContactNo = client.ContactNo;
            clientObj.Email = client.Email;
            clientObj.Address = client.Address;
            clientObj.Website = client.Website;
            clientObj.ProjectId = client.ProjectId;
            clientObj.ModifiedDate = DateTime.Now;
            clientObj.ModifiedBy = User.Identity.Name;
            clientObj.Status = true;

            context.Entry(clientObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("ViewProjectDetails", "Project", new { id = clientObj.ProjectId });
        }

        public ActionResult Next(int id, string mes)
        {
            if (mes == "yes")
            {
                return RedirectToAction("AddContract", "Contract", new { id = id });
            }
            else
            {
                return RedirectToAction("AddTeamLead", "TeamLead", new { id = id });
            }
        }
    }
}