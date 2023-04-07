using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hammal.Utilities
{
    public class EmailSender : IEmailSender
    {
		public string SendGridSecret { get; set; }
		public EmailSender(IConfiguration _config)
		{
			SendGridSecret = _config.GetValue<string>("SendGrid:SecretKey");
		}

		

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
			//var client = new SendGridClient(SendGridSecret);
			//var from = new EmailAddress("oguzhanoztrk00@gmail.com", "turkey");
			//var to = new EmailAddress(email);
			//var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
			//return client.SendEmailAsync(msg);

			//var client = new SendGridClient(SendGridSecret);
			//var from = new EmailAddress("test@example.com", "Example User");
			
			//var to = new EmailAddress("test@example.com", "Example User");
			//var plainTextContent = "and easy to do anywhere with C#.";
			//var htmlContent = "<strong>and easy to do anywhere with C#.</strong>";
			//var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			//var response = client.SendEmailAsync(msg);
			//return response;

			
				string smtpServer = "smtp.office365.com"; //SMTP sunucusu adresi
				int port = 587; //SMTP sunucusu port numarası
				string senderEmail = "hammalexample@hotmail.com"; //Gönderen e-posta adresi
				string password = "O.o123456"; //Gönderen e-posta hesabının şifresi
				string recipientEmail = email; //Alıcı e-posta adresi

				MailMessage mail = new MailMessage();
				mail.From = new MailAddress(senderEmail);
				mail.To.Add(recipientEmail);
				mail.Subject = subject; //E-posta konusu
				mail.IsBodyHtml = true;//HTML email  

				mail.Body = htmlMessage; //E-posta içeriği

				SmtpClient smtpClient = new SmtpClient(smtpServer, port);
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = new NetworkCredential(senderEmail, password);
				smtpClient.EnableSsl = true;

				smtpClient.Send(mail);
				Console.WriteLine("E-posta gönderildi.");
				return Task.CompletedTask;
			

		}




	}
}
