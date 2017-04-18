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
using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

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
                UseDefaultCredentials = settings.Network.DefaultCredentials,
                Credentials = new NetworkCredential(settings.Network.UserName, settings.Network.Password)
            };

            return smtpClient;
        }

        public async Task SendGrid(string toAddresses, string userName, string subject, string body)
        {
            try
            {
                var apiKey ="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";// System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");

                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("pycogroup.chatbot@pycogroup.com", "Pyco Bot Chat");
             
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

            using (var message = new MailMessage(new MailAddress(string.IsNullOrEmpty(SenderEmail) ? "bupaad2016@gmail.com" : SenderEmail), new MailAddress(toAddresses)))
            {
                message.Subject = subject;
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


        private const string AuthenticationParam = "api";
        private const string DomainKey = "domain";
        private const string ResourceType = "{domain}/messages";
        private const string DefaultEmailSender = "admin@dice.com";

        public void Send(string toAddress, string subject, string body)
        {
            var response = SendSimpleMessage(toAddress, subject, body);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Send mail is not delivery successfully. ErrorCode: " + response.StatusCode + " ." + response.StatusDescription);
            }
        }

        private static IRestResponse SendSimpleMessage(string to, string subject, string message)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator(AuthenticationParam, "key-dd7c647c0545d46e21c918c9015bb9cb");
            var request = GetRestRequest(to, subject, message);
            return client.Execute(request);
        }

        private static RestRequest GetRestRequest(string toAddress, string subject, string message)
        {
            var request = new RestRequest();
            request.AddParameter(DomainKey, "sandbox8c985982edf74edba0f8106d50d59037.mailgun.org", ParameterType.UrlSegment);
            request.Resource = ResourceType;

            request.AddParameter("from", "admin@dice.com");
            request.AddParameter("to", toAddress);
            request.AddParameter("subject", subject);
            request.AddParameter("html", message);
            request.Method = Method.POST;

            return request;
        }
    }
}