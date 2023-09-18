namespace AccessControlApplication.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string receiver, string subject, string body);
        Task SendMail(string emailaddress, string message);
        string EmailMessage(string fullname, string userId, string idCardNumber);

    }
}
