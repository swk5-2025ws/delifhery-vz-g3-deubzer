namespace DeliFHery.API.Interfaces
{
    public interface IEmailSender
    {
       public Task SendAsync(string toEmail, string subject, string body, CancellationToken ct);
    }
}
