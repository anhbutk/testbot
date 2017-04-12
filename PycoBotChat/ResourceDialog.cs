using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text;
using System.Collections.Generic;
using PycoBotChat.Helpers;
using Resources;
using PycoBotChat.Models;
using System.Globalization;

namespace PycoBotChat
{
    [Serializable]
    public class ResourceDialog : IDialog<object>
    {
        string strBaseURL;
        public static readonly List<string> Department = new List<string> { "JAVA", "NET", "PHP", "QC", "PM", "HTML", "Mobile", "BA" };
        protected int intNumberToGuess;
        protected int intAttempts;

        #region public async Task StartAsync(IDialogContext context)
        public async Task StartAsync(IDialogContext context)
        {      
            context.Wait(MessageReceivedAsync);
        }
        #endregion

        public virtual async Task MessageReceivedAsync(
            IDialogContext context,
            IAwaitable<IMessageActivity> argument)
        {
            // Set BaseURL
            context.UserData.TryGetValue<string>(
                "CurrentBaseURL", out strBaseURL);

               // Get the text passed
            var message = await argument;
            if (Department.Contains(message.Text))
            {
                //this.Database.SqlQuery<YourEntityType>("storedProcedureName",params);
                Models.BotDataEntities DB = new Models.BotDataEntities();
                string deparment = message.Text.Equals("HTML") ? "FE" : message.Text;
                var resourceList = DB.RESOURCE_VIEW.Where(x => x.STATUS == "On" && x.COMPANY == "Inhouse" && x.DEPTNAME == deparment).ToList();
                Activity replyToConversation = BuildResourceList(context, resourceList, deparment);

                await context.PostAsync(replyToConversation);
                context.Wait(MessageReceivedAsync);
            }
            else if (message.Text.Contains("search:"))
            {
                string fullname = message.Text.Substring(7);
                Models.BotDataEntities DB = new Models.BotDataEntities();
                var currentCulture = CultureInfo.CurrentCulture;
                int weekNo = currentCulture.Calendar.GetWeekOfYear(
                                DateTime.Now,
                                currentCulture.DateTimeFormat.CalendarWeekRule,
                                currentCulture.DateTimeFormat.FirstDayOfWeek);
                var detail = DB.C003_ALLOCATION_WEEK_VIEW.Where(x => x.STATUS == "On" && x.FULLNAME == fullname && x.Year.Value == DateTime.Now.Year && x.Week.Value == weekNo).ToList();
                Activity replyToConversation = BuildDetailResource(context, detail, fullname);

                await context.PostAsync(replyToConversation);
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                // Create a response
                // This time call the ** ShowButtons ** method
                Activity replyToConversation = UIControl.
                    ShowButtons(context, "It's not a department or resource name in PycoGroup.Please choice again.");
                await context.PostAsync(replyToConversation);
                context.Wait(MessageReceivedAsync);
            }

           
            //        sb.Append("Would you like to play again?");

            //        string CongratulationsStringPrompt =
            //            string.Format(sb.ToString(),
            //            this.intNumberToGuess,
            //            this.intAttempts);

            
            //        // Create a reply Activity
            //        Activity replyToConversation = (Activity)context.MakeMessage();
                

            //        // Put PromptDialog here
            //        PromptDialog.Confirm(
            //            context,
            //            PlayAgainAsync,
            //            CongratulationsStringPrompt,
            //            "Didn't get that!");
            //    }
            //}
        }

        private Activity BuildDetailResource(IDialogContext context, List<C003_ALLOCATION_WEEK_VIEW> resourceList , string fullname)
        {
            string noAssigned = $"{fullname} is available this week.";
            string strReplyMessage = $"{fullname} are working on **{resourceList.Count}** projects this week:\n\n";
            foreach (C003_ALLOCATION_WEEK_VIEW item in resourceList)
            {
                strReplyMessage += $"Project:**{item.PROJECTNAME}**, Hours: {item.Hours_RMD.Value.ToString()}.\n\n";
               
            }
            Activity replyToConversation = (Activity)context.MakeMessage();
            replyToConversation.Text = resourceList.Count> 0 ? strReplyMessage : noAssigned;
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";
            return replyToConversation;
        }

        private Activity BuildResourceList(IDialogContext context, List<RESOURCE_VIEW> resourceList, string department)
        {
            HeroCard plCard = new HeroCard()
            {
                Buttons = new List<CardAction>()
            };

            string strReplyMessage = $"There are **{resourceList.Count}** members in {department} team:\n\n";
            foreach (RESOURCE_VIEW item in resourceList)
            {
                strReplyMessage += $"Name:**{item.FULLNAME}**, {item.TITLE}, Skype: skype:{item.SKYPE}?chat, Phone:**{item.PHONE}**.\n\n";
                plCard.Buttons.Add(UIControl.DetailResource(item.FULLNAME, item.USERNAME));
            }
            strReplyMessage += $"Confluence space: {ConfluenceTeam(department)}.\n\n";
            // Create a reply Activity
            Activity replyToConversation = (Activity)context.MakeMessage();
            replyToConversation.Text = strReplyMessage;
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";
            //Create an Attachment
            //set the AttachmentLayout as 'list'
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);
            replyToConversation.AttachmentLayout = "list";

            return replyToConversation;
        }

        private string ConfluenceTeam(string team)
        {
            string url = string.Empty;
            switch (team) {
                case "JAVA":
                    url= "https://apps.pyramid-consulting.com/docs/display/JAVA/JAVA+Team";
                    break;
                case "NET":
                    url = "https://apps.pyramid-consulting.com/docs/display/TMS/.NET";
                    break;
                case "FE":
                    url = "https://apps.pyramid-consulting.com/docs/display/FnM";
                    break;
                case "PHP":
                    url = "https://apps.pyramid-consulting.com/docs/display/PHP/Dashboard";
                    break;
                case "PM":
                    url = "https://apps.pyramid-consulting.com/docs/display/PMTEAM";
                    break;
                case "BA":
                    url = "https://apps.pyramid-consulting.com/docs/display/BCT";
                    break;
                case "QC":
                    url = "https://apps.pyramid-consulting.com/docs/display/QT/QC+Home";
                    break;
                case "Mobile":
                    url = "https://apps.pyramid-consulting.com/docs/display/MOBILE";
                    break;
            }
            return url;
        }

        private async Task PlayAgainAsync(IDialogContext context, IAwaitable<bool> result)
        {
            // Generate new random number
            Random random = new Random();
            this.intNumberToGuess = random.Next(1, 6);

            // Reset attempts
            this.intAttempts = 1;

            // Get the response from the user
            var confirm = await result;

            if (confirm) // They said yes
            {
                // Start a new Game
                // Create a response
                // This time call the ** ShowButtons ** method
                Activity replyToConversation = UIControl.
                    ShowButtons(context,
                    "Hi Welcome! - Guess a number between 1 and 5 \n\n Type 'High Scores' to see high scores");


                await context.PostAsync(replyToConversation);
                context.Wait(MessageReceivedAsync);
            }
            else // They said no
            {
                await context.PostAsync("Goodbye!(heart)");
                context.Wait(MessageReceivedAsync);
            }
        }

        // Utility

      
    }
}