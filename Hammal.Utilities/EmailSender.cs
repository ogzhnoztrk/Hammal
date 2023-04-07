using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

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

			//Burası Çalışıyor mail adresleri ile uğraşmamak için kapattım
			//Sadece Secret Yönetimi yapılacak

			//string smtpServer = "smtp.office365.com"; //SMTP sunucusu adresi
			//int port = 587; //SMTP sunucusu port numarası
			//string senderEmail = "hammalexample@hotmail.com"; //Gönderen e-posta adresi
			//string password = "O.o123456"; //Gönderen e-posta hesabının şifresi
			//string recipientEmail = email; //Alıcı e-posta adresi

			//MailMessage mail = new MailMessage();
			//mail.From = new MailAddress(senderEmail);
			//mail.To.Add(recipientEmail);
			//mail.Subject = subject; //E-posta konusu
			//mail.IsBodyHtml = true;//HTML email  

			//mail.Body = htmlMessage; //E-posta içeriği

			//SmtpClient smtpClient = new SmtpClient(smtpServer, port);
			//smtpClient.UseDefaultCredentials = false;
			//smtpClient.Credentials = new NetworkCredential(senderEmail, password);
			//smtpClient.EnableSsl = true;

			//smtpClient.Send(mail);
			//Console.WriteLine("E-posta gönderildi.");


			return Task.CompletedTask;


		}




	}
}
