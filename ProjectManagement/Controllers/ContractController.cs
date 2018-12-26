using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();
        public ActionResult AddContract(int id)
        {
            var contracts = context.Contractors.Where(p => p.ProjectId == id && p.Status == true).ToList();
            var client = context.Clients.Where(p => p.Status == true).ToList();
            var projects = context.Projects.Where(p => p.Id == id && p.Status == true).ToList();

            ViewData["clients"] = client;
            ViewData["projects"] = projects;

            return View(contracts);
        }

        public ActionResult SaveContract(Contractor contract)
        {
            var addressObject = new AddressDetail
            {
                Id = contract.AddressId,
                Street = contract.AddressDetail.Street,
                City = contract.AddressDetail.City,
                Province = contract.AddressDetail.Province,
                Country = contract.AddressDetail.Country,
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name
            };

            context.AddressDetails.Add(addressObject);
            context.SaveChanges();

            var contractObj = new Contractor
            {
                Id = contract.Id,
                ProjectId = contract.ProjectId,
                Description = contract.Description,
                AddressId = addressObject.Id,
                RepresentiveName = contract.RepresentiveName,
                RepresentiveContact = contract.RepresentiveContact,
                RepresentiveEmail = contract.RepresentiveEmail,
                RepresentivePhone = contract.RepresentivePhone,
                CreatedDate = DateTime.Now,
                CreatedBy = User.Identity.Name,
                Status = true
            };

            context.Contractors.Add(contractObj);
            context.SaveChanges();

            //return RedirectToAction("AddTeamLead", "TeamLead", new { id = contractObj.ProjectId });
            return RedirectToAction("AddContract", new { id = contractObj.ProjectId });
        }


        public ActionResult DeleteContract(int id)
        {
            var contract = context.Contractors.Find(id);
            contract.Status = false;
            context.Entry(contract).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("ViewProjectDetails", "Project", new { id = contract.ProjectId });
        }

        public ActionResult ContractUpdate(int id, int projectId)
        {
            var contract = context.Contractors.Where(p => p.Id == id && p.Status == true).FirstOrDefault();
            var client = context.Clients.Where(p => p.Status == true).ToList();
            var project = context.Projects.Where(p => p.Id == projectId && p.Status == true).ToList();
            ViewData["client"] = client;
            ViewData["project"] = project;

            return View(contract);
        }

        public ActionResult UpdateContract(Contractor contract)
        {
            var addressObject = context.AddressDetails.Find(contract.AddressDetail.Id);
            addressObject.Id = contract.AddressDetail.Id;
            addressObject.Street = contract.AddressDetail.Street;
            addressObject.City = contract.AddressDetail.City;
            addressObject.Province = contract.AddressDetail.Province;
            addressObject.Country = contract.AddressDetail.Country;
            addressObject.ModifiedDate = DateTime.Now;
            addressObject.ModifiedBy = User.Identity.Name;

            context.Entry(addressObject).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            var contractObj = context.Contractors.Find(contract.Id);
            contractObj.Id = contract.Id;
            contractObj.ProjectId = contract.ProjectId;
            contractObj.Description = contract.Description;
            contractObj.AddressId = addressObject.Id;
            contractObj.RepresentiveName = contract.RepresentiveName;
            contractObj.RepresentiveContact = contract.RepresentiveContact;
            contractObj.RepresentiveEmail = contract.RepresentiveEmail;
            contractObj.RepresentivePhone = contract.RepresentivePhone;
            contractObj.ModifiedDate = DateTime.Now;
            contractObj.ModifiedBy = User.Identity.Name;

            context.Entry(contractObj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("ViewProjectDetails", "Project", new { id = contractObj.ProjectId });
        }

        public ActionResult Next(int id)
        {
            return RedirectToAction("AddTeamLead", "TeamLead", new { id = id });
        }
    }
}