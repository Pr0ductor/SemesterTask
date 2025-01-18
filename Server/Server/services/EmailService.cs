using System.Net;
using System.Net.Mail;
using HttpServerLibrary.Configurations;


namespace MyServer.services;

public class EmailService : IEmailService
{
    private EmailServiceConfig config;
    public EmailService(EmailServiceConfig config)
    {
        this.config = config;
    }
    public void SendEMail(string email, string subject, string message)
    {
        string fromEmail = "Gre4ka28@yandex.ru";
        MailAddress from = new MailAddress(fromEmail, "Evgeny");
        MailAddress to = new MailAddress(email);
    
        MailMessage m = new MailMessage(from, to);
        m.Subject = subject;
        m.Body = message;
        m.IsBodyHtml = true;
    
        SmtpClient smtp = new SmtpClient(config.Host, config.Port);
        smtp.Credentials = new NetworkCredential(config.UserName, config.Password);
        smtp.EnableSsl = config.IsSSL;

        try
        {
            smtp.Send(m);
            Console.WriteLine("Письмо отправлено!");
            Console.WriteLine(config.UserName);
            Console.WriteLine(config.Password);
            Console.WriteLine(config.IsSSL);
            Console.WriteLine(config.Port);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}