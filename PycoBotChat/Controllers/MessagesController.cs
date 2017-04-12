using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using System.Collections.Generic;
using System.Text;
using Microsoft.Bot.Builder.FormFlow;
using System.Net.Mail;
using PycoBotChat.Helpers;
using System.Data.Entity;
using PycoBotChat.Models;
using Resources;

namespace PycoBotChat
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            bool boolAskedForUserName = false;
            string strUserName = "";
            #region Set CurrentBaseURL and ChannelAccount
            // Get the base URL that this service is running at
            // This is used to show images
            string CurrentBaseURL =
                    this.Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");

            // Create an instance of BotData to store data
            BotData objBotData = new BotData();

            // Instantiate a StateClient to save BotData            
            StateClient stateClient = activity.GetStateClient();

            // Use stateClient to get current userData
            BotData userData = await stateClient.BotState.GetUserDataAsync(
                activity.ChannelId, activity.From.Id);

            // Update userData by setting CurrentBaseURL and Recipient
            userData.SetProperty<string>("CurrentBaseURL", CurrentBaseURL);

            // Save changes to userData
            await stateClient.BotState.SetUserDataAsync(
                activity.ChannelId, activity.From.Id, userData);
            #endregion
           
            if (activity.Type == ActivityTypes.Message)
            {
                BotData leadData = stateClient.BotState.GetPrivateConversationData(
                 activity.ChannelId, activity.Conversation.Id, activity.From.Id);               

                Models.BotDataEntities DB = new Models.BotDataEntities();
                User user = DB.Users.FirstOrDefault(x => x.UserID == activity.From.Id);
                StringBuilder strReplyMessage = new StringBuilder();
                if (user == null)
                {
                    boolAskedForUserName = leadData.GetProperty<bool>("AskedForUserName");
                    strUserName = leadData.GetProperty<string>("Email") ?? "";
                    // Create text for a reply message   

                    if (boolAskedForUserName == false) // Never asked for email
                    {
                        strReplyMessage.Append(SR.Hello);
                        strReplyMessage.Append($"\n\n");
                        strReplyMessage.Append(SR.WhatEmail);

                        // Set BotUserData
                        leadData.SetProperty<bool>("AskedForUserName", true);
                    }
                    else // Have asked for email
                    {
                        string[] emailDomain = { "@pycogroup.com", "@pyramid-consulting", "@pyco.be", "@pyco-group.com" };
                        if (!emailDomain.Any(x=>activity.Text.Contains(x))) // Name was never provided
                        {
                            // If we have asked for a username but it has not been set
                            // the current response is the user name
                            strReplyMessage.Append(SR.PycoEmailWrong);

                        }
                        else // Name was provided
                        {
                            string email = new EmailHelper().ExtractEmails(activity.Text);
                            leadData.SetProperty<string>("Email", email);
                            strReplyMessage.Append(string.Format(SR.OTPSend, email));                            
                            string newpass = SendOTP(activity, email);
                            user = new User()
                            {
                                Channel = activity.ChannelId,
                                UserID = activity.From.Id,
                                UserName = activity.From.Name,
                                ExpiredOTP = DateTime.UtcNow.AddHours(1),
                                IsVerified = false,
                                Email = email,
                                OTP = newpass,
                                JoinDate = DateTime.UtcNow
                            };
                            DB.Users.Add(user);
                            DB.SaveChanges();
                        }
                    }

                    // Save BotUserData
                    stateClient.BotState.SetPrivateConversationData(
                        activity.ChannelId, activity.Conversation.Id, activity.From.Id, leadData);

                    // Create a reply message
                    ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                    Activity replyMessage = activity.CreateReply(strReplyMessage.ToString());
                    await connector.Conversations.ReplyToActivityAsync(replyMessage);
                }
                else //check OTP is ok or user already authorize
                {
                    if (user.IsVerified)
                    {
                        await Conversation.SendAsync(activity, () => new ResourceDialog());
                    }
                    else
                    {
                        if (user.OTP == activity.Text && user.ExpiredOTP > DateTime.UtcNow)
                        {
                            //Update Database
                            user.IsVerified = true;
                            DB.Users.Attach(user);
                            DB.Entry(user).State = EntityState.Modified;
                            // other changed properties
                            DB.SaveChanges();                            
                            strReplyMessage.Append(string.Format(SR.OTPValid, activity.From.Name));
                            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                            Activity replyMessage = activity.CreateReply(strReplyMessage.ToString());

                            // Call the CreateButtons utility method 
                            // that will create 5 buttons to put on the Here Card
                            List<CardAction> cardButtons = UIControl.CreateButtons();

                            // Create a Hero Card and add the buttons 
                            HeroCard plCard = new HeroCard()
                            {
                                Buttons = cardButtons
                            };

                            // Create an Attachment
                            // set the AttachmentLayout as 'list'
                            Microsoft.Bot.Connector.Attachment plAttachment = plCard.ToAttachment();
                            replyMessage.Attachments.Add(plAttachment);
                            replyMessage.AttachmentLayout = "list";


                            await connector.Conversations.ReplyToActivityAsync(replyMessage);
                        }
                        else if (user.OTP != activity.Text && user.ExpiredOTP > DateTime.UtcNow)
                        {
                            strReplyMessage.Append(SR.OTPWrong);
                            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                            Activity replyMessage = activity.CreateReply(strReplyMessage.ToString());
                            await connector.Conversations.ReplyToActivityAsync(replyMessage);
                        }
                        else // OTP is wrong and expired
                        {
                            strReplyMessage.Append(SR.OTPInvalid);
                            DB.Users.Remove(user);
                            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                            Activity replyMessage = activity.CreateReply(strReplyMessage.ToString());
                            await connector.Conversations.ReplyToActivityAsync(replyMessage);
                        }
                      
                    }
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private string SendOTP(Activity activity, string email)
        {
            string newpass = new EmailHelper().GenerateOTP();
            string body = string.Format(SR.EmailOTP, newpass); 
            new EmailHelper().SendGrid(email, activity.From.Name, SR.EmailSubject, body);
            return newpass;
        }


        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                SaveLog(message);
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Construct a base URL for Image
                // To allow it to be found wherever the application is deployed
                string strCurrentURL =
                    this.Url.Request.RequestUri.AbsoluteUri.Replace(@"api/messages", "");

                // Create a reply message
                Activity replyToConversation = message.CreateReply();
                replyToConversation.Recipient = message.From;
                replyToConversation.Type = "message";
                replyToConversation.Attachments = new List<Microsoft.Bot.Connector.Attachment>();
                // AttachmentLayout options are list or carousel
                replyToConversation.AttachmentLayout = "carousel";

                #region Card One
                // Full URL to the image
                string strNumberGuesserOpeningCard =
                    String.Format(@"{0}/{1}",
                    strCurrentURL,
                    "Images/Pyco.png");

                // Create a CardImage and add our image
                List<CardImage> cardImages1 = new List<CardImage>();
                cardImages1.Add(new CardImage(url: strNumberGuesserOpeningCard));

                // Create a CardAction to make the HeroCard clickable
                // Note this does not work in some Skype clients
                CardAction btnAiHelpWebsite = new CardAction()
                {
                    Type = "openUrl",
                    Title = "PycoGroup.com",
                    Value = "http://www.pycogroup.com"
                };

                // Finally create the Hero Card
                // adding the image and the CardAction
                HeroCard plCard1 = new HeroCard()
                {
                    Title = SR.WelcomeTilte,
                    Subtitle = SR.PycoTerm,
                    Images = cardImages1,
                    Tap = btnAiHelpWebsite
                };

                // Create an Attachment by calling the
                // ToAttachment() method of the Hero Card
                Microsoft.Bot.Connector.Attachment plAttachment1 = plCard1.ToAttachment();

                // Add the Attachment to the reply message
                replyToConversation.Attachments.Add(plAttachment1);
                #endregion
                

                // Create a ConnectorClient and use it to send the reply message
                var connector =
                    new ConnectorClient(new Uri(message.ServiceUrl));
                var reply =
                    connector.Conversations.SendToConversationAsync(replyToConversation);
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing that the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }


        private void SaveLog(Activity activity)
        {
            // *************************
            // Log to Database
            // *************************

            // Instantiate the BotData dbContext
            Models.BotDataEntities DB = new Models.BotDataEntities();
            // Create a new UserLog object
            Models.UserLog NewUserLog = new Models.UserLog();

            // Set the properties on the UserLog object
            NewUserLog.Channel = activity.ChannelId;
            NewUserLog.UserID = activity.From.Id;
            NewUserLog.UserName = activity.From.Name;
            NewUserLog.created = DateTime.UtcNow;
            NewUserLog.Message = activity.Text.Truncate(500);

            try
            {
                // Add the UserLog object to UserLogs
                DB.UserLogs.Add(NewUserLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Save the changes to the database
            DB.SaveChanges();
        }
        
     
    }
}