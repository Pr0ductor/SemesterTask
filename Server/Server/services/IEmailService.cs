namespace MyServer.services;

public interface IEmailService
{
    void SendEMail(string email, string subject, string message);
}