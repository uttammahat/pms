using ProjectManagement.Common;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProjectManagement.Controllers
{
    public class AccountController : Controller
    {
        private ProjectManagementEntities context = new ProjectManagementEntities();


        public ActionResult Login()
        {
            if (TempData["errMess"] != null)
            {
                ViewData["InvalidLogin"] = TempData["errMess"];
            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        //public ActionResult RegisterAccount(Account account)
        //{
        //    var accountObj = new Account
        //    {
        //        FirstName = account.FirstName,
        //        LastName = account.LastName,
        //        Email = account.Email,
        //        Username = account.Username,
        //        Password = HashPassword.Encode(account.Password)
        //        //Password = account.Password
        //    };

        //    try
        //    {
        //        context.Accounts.Add(accountObj);
        //        context.SaveChanges();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    return RedirectToAction("Index", "Home");
        //}

        public ActionResult LoginUser(Login login)
        {
            var encodedPassword = HashPassword.Encode(login.Password);
            //var encodedPassword = login.Password;
            var loginData = context.Employees.Where(p => p.Username == login.Username && p.Password == encodedPassword && p.Status == true).SingleOrDefault();

            if (loginData != null)
            {
                FormsAuthentication.SetAuthCookie(login.Username, false);
                
                //if (loginData.Type == "User")
                //{
                //    var data = context.LoggedInUsers.ToList();

                //    if (data.Count > 0)
                //    {
                //        var users = data[0];
                //        users.LoggedInUserId = loginData.Id;
                //        context.Entry(data[0]).State = System.Data.Entity.EntityState.Modified;
                //    } else
                //    {
                //        var loggedInUserObj = new LoggedInUser
                //        {
                //            LoggedInUserId = loginData.Id
                //        };

                //        context.LoggedInUsers.Add(loggedInUserObj);
                //        context.SaveChanges();
                //    }
                //}

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["errMess"] = "Invalid credential";
                return RedirectToAction("Login");
            }
        }

        [Authorize]
        public ActionResult LogoutUser()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult ForgotPassword()
        {
            if (TempData["message"] != null)
            {
                ViewData["message"] = TempData["message"];
            }
            return View();
        }

        public ActionResult ChangePassword(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public async Task<ActionResult> SendInvitationLinkAsync(FormCollection fc)
        {
            string mail = fc["Email"];
            string root = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");

            var accounts = context.Employees.Where(p => p.Email == mail).SingleOrDefault();
            if (accounts != null)
            {
                string subject = "Please recover your password";
                string message = "<p>Please click <a href='" + root + "/Account/ChangePassword/" + accounts.Id + "'>here</a> to recover your password</p>";
                string recepient = mail;

                await MailSender.SendEmail(subject, message, recepient);
                TempData["message"] = "Mail send";
            }
            else
            {
                TempData["message"] = "Invalid email address";
            }

            return RedirectToAction("ForgotPassword");
        }

        public ActionResult UpdatePassword(FormCollection fc)
        {
            var account = context.Employees.Find(Convert.ToInt32(fc["Id"]));
            account.Password = HashPassword.Encode(fc["Password"]);
            context.Entry(account).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Login");
        }

        public ActionResult AccountActivation(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        public ActionResult ActiveAccount(FormCollection fc)
        {
            int id = Convert.ToInt32(fc["id"]);
            string password = HashPassword.Encode(fc["Password"]);

            var res = context.Employees.Find(id);
            res.Status = true;
            res.Password = password;

            context.Entry(res).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("Login");
        }

    }
}