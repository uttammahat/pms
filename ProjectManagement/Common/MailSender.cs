using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace ProjectManagement.Common
{
    public class MailSender
    {
        public async static Task SendEmail(string subject, string message, string recepient)
        {
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("nishandhungana41@gmail.com", "*Nishan@44248#")
            };

            using (var mess = new MailMessage("nishandhungana41@gmail.com", recepient)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            })
            {
                await smtpClient.SendMailAsync(mess);
            }
        }
    }
}