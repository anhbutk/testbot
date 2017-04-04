using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PycoBotChat.Helpers
{
    public class EmailHelper
    {
         private SmtpClient smtpClient;
        #region Public Properties

        public string SenderEmail { get; set; }

        #endregion
        public string GenerateOTP() {

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = string.Empty;
            characters = alphabets + small_alphabets + numbers;
            string otp = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

        private SmtpClient GetDefaultSmtpClient()
        {
            var settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

            SenderEmail = settings.From;

            var smtpClient = new SmtpClient
            {
                Host = settings.Network.Host,
                Port = settings.Network.Port,
                EnableSsl = settings.Network.EnableSsl,
                Credentials = new NetworkCredential(settings.Network.UserName, settings.Network.Password)
            };

            return smtpClient;
        }

        public async void SendGrid(string toAddresses, string userName, string subject, string body)
        {
            try
            {
                var apiKey = "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";// System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("anh.nguyenquang@pycogroup.com", "Anh Nguyen");
             
                var to = new EmailAddress(toAddresses, userName);
            
                var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public void Send(string toAddresses, string ccAddresses, string bccAdresses, string subject, string body)
        {
            if (smtpClient == null)
            {
                smtpClient = GetDefaultSmtpClient();
            }

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(SenderEmail);
                message.To.Add(toAddresses);
                if (!string.IsNullOrEmpty(ccAddresses))
                    message.CC.Add(ccAddresses);
                if (!string.IsNullOrEmpty(bccAdresses))
                    message.Bcc.Add(bccAdresses);
                message.Subject = "=?UTF-8?B?" + Convert.ToBase64String(Encoding.UTF8.GetBytes(subject)) + "?=";
                message.Body = body;
                message.IsBodyHtml = true;

                smtpClient.Send(message);
            }
        }

        public string ExtractEmails(string textToScrape)
        {
            Regex reg = new Regex(@"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}", RegexOptions.IgnoreCase);
            Match match;

            List<string> results = new List<string>();
            for (match = reg.Match(textToScrape); match.Success; match = match.NextMatch())
            {
                if (!(results.Contains(match.Value)))
                    results.Add(match.Value);
            }

            return results[0];
        }
    }
}