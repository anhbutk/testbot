using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace PycoBotChat.Helpers
{

    public static class UIControl
    {
        public static readonly List<string> Department = new List<string> { "JAVA", "NET", "PHP", "QC", "PM", "HTML", "Mobile", "BA" };
        public static List<CardAction> CreateButtons()
        {

            // Create 5 CardAction buttons 
            // and return to the calling method 
            List<CardAction> cardButtons = new List<CardAction>();
            for (int i = 0; i < Department.Count; i++)
            {
                CardAction CardButton = new CardAction()
                {
                    Type = "imBack",
                    Title = Department[i],
                    Value = Department[i]
                };

                cardButtons.Add(CardButton);
            }

            return cardButtons;
        }

        #region private static Activity ShowButtons(IDialogContext context, string strText)
        public static Activity ShowButtons(IDialogContext context, string strText)
        {
            // Create a reply Activity
            Activity replyToConversation = (Activity)context.MakeMessage();
            replyToConversation.Text = strText;
            replyToConversation.Recipient = replyToConversation.Recipient;
            replyToConversation.Type = "message";

            // Call the CreateButtons utility method 
            // that will create 5 buttons to put on the Here Card
            List<CardAction> cardButtons = CreateButtons();

            // Create a Hero Card and add the buttons 
            HeroCard plCard = new HeroCard()
            {
                Buttons = cardButtons
            };

            // Create an Attachment
            // set the AttachmentLayout as 'list'
            Attachment plAttachment = plCard.ToAttachment();
            replyToConversation.Attachments.Add(plAttachment);
            replyToConversation.AttachmentLayout = "list";

            // Return the reply to the calling method
            return replyToConversation;
        }
        #endregion
    }
}