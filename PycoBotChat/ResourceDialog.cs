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

            // See if a number was passed
            //if (!int.TryParse(message.Text, out intGuessedNumber))
            //{
            //    // A number was not passed  

            //    // Create a reply Activity
            //    Activity replyToConversation = (Activity)context.MakeMessage();
            //    replyToConversation.Recipient = replyToConversation.Recipient;
            //    replyToConversation.Type = "message";

            //    string strLogo =
            //        String.Format(@"{0}/{1}",
            //        strBaseURL,
            //        "Images/Pycologo.png");

            //    List<CardImage> cardImages = new List<CardImage>();
            //    cardImages.Add(new CardImage(url: strLogo));

            //    // Create the Buttons
            //    // Call the CreateButtons utility method
            //    List<CardAction> cardButtons = CreateButtons();

            //    // Create the Hero Card
            //    // Set the image and the buttons
            //    HeroCard plCard = new HeroCard()
            //    {
            //        Images = cardImages,
            //        Buttons = cardButtons,
            //    };

            //    // Create an Attachment by calling the
            //    // ToAttachment() method of the Hero Card                
            //    Attachment plAttachment = plCard.ToAttachment();
            //    // Attach the Attachment to the reply
            //    replyToConversation.Attachments.Add(plAttachment);
            //    // set the AttachmentLayout as 'list'
            //    replyToConversation.AttachmentLayout = "list";

            //    // Send the reply
            //    // Create text for a reply message   
            //    replyToConversation.Text = string.Format(SR.Welcome, message.From.Name);
                
            //    await context.PostAsync(replyToConversation);
            //    context.Wait(MessageReceivedAsync);
            //}

            // This code will run when the user has entered a number
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
            else
            {
                // Create a response
                // This time call the ** ShowButtons ** method
                Activity replyToConversation = UIControl.
                    ShowButtons(context, "It's not a department.Please choice again.");
                await context.PostAsync(replyToConversation);
                context.Wait(MessageReceivedAsync);
            }

            //if (int.TryParse(message.Text, out intGuessedNumber))
            //{
            //    // A number was passed
            //    // See if it was the correct number
            //    if (intGuessedNumber != this.intNumberToGuess)
            //    {
            //        // The number was not correct
            //        this.intAttempts++;

            //        // Create a response
            //        // This time call the ** ShowButtons ** method
            //        Activity replyToConversation =
            //            ShowButtons(context, "Not correct. Guess again.");

               

            //        await context.PostAsync(replyToConversation);
            //        context.Wait(MessageReceivedAsync);
            //    }
            //    else
            //    {
            //        // Game completed
            //        StringBuilder sb = new StringBuilder();
            //        sb.Append("Congratulations! ");
            //        sb.Append("The number to guess was {0}. ");
            //        sb.Append("You needed {1} attempts. ");
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

        private Activity BuildResourceList(IDialogContext context, List<RESOURCE_VIEW> resourceList, string department)
        {
            string strReplyMessage = $"There are **{resourceList.Count}** members in {department} team:\n\n";
            foreach (RESOURCE_VIEW item in resourceList)
            {
                strReplyMessage += $"Name:**{item.FULLNAME}**, {item.TITLE}, Skype: skype:{item.SKYPE}?chat, Phone:**{item.PHONE}**.\n\n";
            }
            strReplyMessage += $"Confluence space: {ConfluenceTeam(department)}.\n\n";
            // Create a reply Activity
            Activity replyToConversation = (Activity)context.MakeMessage();
            replyToConversation.Text = strReplyMessage;
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";
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
                case "HTML":
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