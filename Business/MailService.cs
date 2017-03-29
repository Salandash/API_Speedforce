using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API_Speedforce.Models;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace API_Speedforce.Business
{
    public class MailService
    {
        public static void SendActivationEmail(User model)
        {
            try
            {
                var message = new MailMessage
                {
                    From = new MailAddress(model.Email),
                    Subject = "Activation Email",
                    Body = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Templates/ActivationEmail.html")),
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8
                };

                message.Body = message.Body.Replace("{NAME}", model.Username);
                message.Body = message.Body.Replace("{PASSWORD}", model.Password);
                message.To.Add(model.Email);

                var client = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = false,
                    UseDefaultCredentials = String.IsNullOrEmpty("wjhg03@gmail.com"),
                };

                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}