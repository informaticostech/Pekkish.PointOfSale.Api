using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Pekkish.PointOfSale.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pekkish.PointOfSale.Api.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string emailTo, string subject, string emailBody);
        Task SendEmailAsync(string emailTo, string subject, string emailBody, List<EmailAttachment> Attachments);
    }    
    public class EmailSender : IEmailSender
    {
        private readonly EmailSetting _emailSettings;

        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSetting> emailSettings, ILogger<EmailSender> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string emailBody)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
                var emailTos = emailTo.Split(";".ToCharArray());
                foreach (var email in emailTos)
                {
                    if (!string.IsNullOrEmpty(email))
                        mimeMessage.To.Add(new MailboxAddress(email, email));
                }

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = emailBody
                };
                if (!string.IsNullOrEmpty(_emailSettings.SpecifiedPickupDirectory))
                {
                    SaveToPickupDirectory(mimeMessage, _emailSettings.SpecifiedPickupDirectory, FormatNameForFile(subject));
                    _logger.LogInformation($"Copied Message to: {_emailSettings.SpecifiedPickupDirectory}");
                }
                else
                {
                    using (var client = new SmtpClient())
                    {
                        // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                        // Note: only needed if the SMTP server requires authentication
                        await client.ConnectAsync(_emailSettings.MailServer, (int)_emailSettings.MailPort, SecureSocketOptions.SslOnConnect);

                        // Note: only needed if the SMTP server requires authentication
                        await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);                                               
                        await client.SendAsync(mimeMessage);
                        await client.DisconnectAsync(true);
                    }
                }


            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task SendEmailAsync(string emailTo, string subject, string emailBody, List<EmailAttachment> Attachments)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            var emailTos = emailTo.Split(";".ToCharArray());
            foreach (var email in emailTos)
            {
                if (!string.IsNullOrEmpty(email))
                    mimeMessage.To.Add(new MailboxAddress(email, email));
            }

            mimeMessage.Subject = subject;
            mimeMessage.Body = new TextPart("html")
            {
                Text = emailBody
            };
            var builder = new BodyBuilder { HtmlBody = emailBody };

            //Fetch the attachments from db
            //considering one or more attachments
            if (Attachments != null)
            {
                foreach (var attachment in Attachments)
                {
                    builder.Attachments.Add(attachment.AttachmentName, attachment.Attachment, ContentType.Parse(attachment.AttachmentType));
                }
            }
            mimeMessage.Body = builder.ToMessageBody();
            if (!string.IsNullOrEmpty(_emailSettings.SpecifiedPickupDirectory))
            {
                SaveToPickupDirectory(mimeMessage, _emailSettings.SpecifiedPickupDirectory, FormatNameForFile(subject));
                _logger.LogInformation($"Copied Message to: {_emailSettings.SpecifiedPickupDirectory}");
            }
            else
            {
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    // The third parameter is useSSL (true if the client should make an SSL-wrapped connection to the server; otherwise, false).
                    //await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    await client.ConnectAsync(_emailSettings.MailServer, (int)_emailSettings.MailPort, SecureSocketOptions.SslOnConnect);

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.FromEmail, _emailSettings.Password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
        }

        private void SaveToPickupDirectory(MimeMessage message, string pickupDirectory, string subject)
        {
            do
            {
                var now = $" {DateTime.Now.Year.ToString()}{DateTime.Now.Month.ToString()}{DateTime.Now.Day.ToString()}-{DateTime.Now.Hour.ToString()}{DateTime.Now.Minute.ToString()}{DateTime.Now.Second.ToString()}";
                
                // Note: this will require that you know where the specified pickup directory is.
                var path = Path.Combine(pickupDirectory, subject + now + ".eml");

                if (File.Exists(path))
                {
                    subject += "_";
                    continue;
                }
                    
                try
                {
                    using (var stream = new FileStream(path, FileMode.CreateNew))
                    {
                        message.WriteTo(stream);
                        return;
                    }
                }
                catch (IOException)
                {
                    // The file may have been created between our File.Exists() check and
                    // our attempt to create the stream.
                }
            } while (true);
        }
        private string FormatNameForFile(string parameter)
        {
            parameter = parameter.Replace(' ', '_');
            parameter = parameter.Replace("'", "");

            return parameter;
        }
    }
}



