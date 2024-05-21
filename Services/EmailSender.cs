using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Innerglow_App.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.



    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    public async Task Execute(string subject, string message, string toEmail)
    {
        string senderEmail = "hconnora@outlook.com";
        string password = "Gethiredintech";

        // Recipient's email address
        string recipientEmail = toEmail;

        // Create and configure the SMTP client
        SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(senderEmail, password);
        MailAddress senderAddress = new MailAddress(senderEmail, "He thong website");
        // Create the email email
        MailMessage email = new MailMessage(senderAddress, new MailAddress(recipientEmail));
        email.Subject = subject;
        email.Body = message;
        email.IsBodyHtml = true;
        client.Send(email);
    }
}