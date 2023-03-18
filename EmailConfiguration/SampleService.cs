using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace EmailConfiguration
{
    public class SampleService
    {
        private readonly ILogger<SampleService> _logger;

        public SampleService(ILogger<SampleService> logger)
        {
            _logger = logger;
        }

        public async Task DoSomethingAsync()
        {
            await Task.Delay(100);
            await SendEmail();
            _logger.LogInformation(
                "Sample Service did something.");
        }

        private async Task<bool> SendEmail()
        {
            try
            {
     
                string smail = "";
                string authToken = "";
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("Test Email For Schedule Mail", smail));
                email.To.Add(MailboxAddress.Parse(""));
                email.Subject = "Test Email For Schedule Mail";
                var builder = new BodyBuilder();
                builder.HtmlBody = "Test Email For Schedule Mail";
                email.Body = builder.ToMessageBody();
      

                var fromEmailPassword = authToken;
                var smtp = new SmtpClient();
                smtp.Connect("", Convert.ToInt32(587), SecureSocketOptions.StartTls);
                smtp.Authenticate(smail, fromEmailPassword);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return true; ;
            }
            catch (Exception ex)
            {
                _logger.LogError("Smtp gmail error occurred:" + ex.ToString());
                throw;
            }
        }
    }
}
