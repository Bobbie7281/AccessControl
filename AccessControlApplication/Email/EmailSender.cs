using System.Net;
using System.Net.Mail;

namespace AccessControlApplication.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string receiver, string subject, string body)
        {
            var sender = "robertellul34@gmail.com";
            var pw = "dugaaaqvkinyyodw";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(sender, pw)
            };
            return client.SendMailAsync(
                new MailMessage(from: sender, to: receiver, subject: subject, body: body));

        }

        public async Task SendMail(string emailAddress, string message)
        {
            var receiver = emailAddress;
            var subject = "LogIn Details";

            try
            {
                await SendEmailAsync(receiver, subject, message);
            }
            catch (Exception) { }
        }

        ///<summary>
        ///Provide Full Name User Id and Id Card Number \n
        ///stringFormat(EmailMessage(), Full Name, UserId, Id Card Number);
        /// </summary>

        public string EmailMessage(string fullname, string userId, string idCardNumber)
        {

            string message = string.Format("Dear {0},\n\nUse the following credentials to log into the system\n\n" +
                "User Id:- {1}\n\nId Card Number:-{2}", fullname, userId, idCardNumber);

            return message;
        }
    }
}
