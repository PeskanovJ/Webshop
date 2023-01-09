using Projekat.BLL.Services.Interfaces;
using Projekat.Shared.Common;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Projekat.BLL.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailConfiguration> _emailConfiguration;

        public EmailService(IOptions<EmailConfiguration> emailConfiguration)
        {
            this._emailConfiguration = emailConfiguration;
        }

        public async Task<bool> SendMailAsync(EmailData emailData)
        {
            MailMessage msg = new MailMessage();
            try
            {
                msg.To.Add(new MailAddress(emailData.To));
                foreach (var ccItem in emailData.CcList)
                {
                    msg.CC.Add(new MailAddress(ccItem));
                }
                msg.From = new MailAddress(_emailConfiguration.Value.From);

                // Subject and multipart/alternative Body
                msg.Subject = emailData.Subject;

                if (emailData.IsContentHtml)
                    msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailData.Content, null, MediaTypeNames.Text.Html));
                else
                    msg.Body = emailData.Content;

                // Init SmtpClient and send

                #region SendGrid
                //SmtpClient smtpClient = new SmtpClient(_emailConfiguration.Value.Host, _emailConfiguration.Value.Port);
                //NetworkCredential credentials = new NetworkCredential(_emailConfiguration.Value.Username, _emailConfiguration.Value.ApiKey);
                //smtpClient.Credentials = credentials;

                //await smtpClient.SendMailAsync(msg);
                #endregion

                #region Gmail
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailConfiguration.Value.Email, _emailConfiguration.Value.Password)
                };

                await smtp.SendMailAsync(msg);
                #endregion
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
