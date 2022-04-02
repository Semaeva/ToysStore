﻿using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ToyStore.EmailServices
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new  MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "toystore.kg"));

            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("wunata95@gmail.com", "jziuqhlmbqpkspxo");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
        }
}
