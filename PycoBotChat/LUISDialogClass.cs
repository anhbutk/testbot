using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PycoBotChat
{
    [LuisModel("{a3e1bb9c-7a51-4740-8dae-69af804b2aa9}", "{71804bba6c3142e3bcea5f951b0fd37c}")]
    [Serializable]
    public class LUISDialogClass : LuisDialog<object>
    {
        #region public async Task None(IDialogContext context, LuisResult result)
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            // Not a match -- Start a new Game
            context.Call(new NumberGuesserDialog(), null);
        }
        #endregion

        #region public async Task HighScores(IDialogContext context, LuisResult result)
        [LuisIntent("HighScores")]
        public async Task HighScores(IDialogContext context, LuisResult result)
        {
            // See if the intent has a > .99 match
            bool boolIntentMatch = false;
            foreach (var objIntent in result.Intents)
            {
                // If the HighScores Intent is detected
                // and it's score is greater than or = to .99 
                // set boolIntentMatch = true
                if (
                    (objIntent.Intent == "HighScores")
                    && (objIntent.Score >= .99f)
                    )
                {
                    boolIntentMatch = true;
                }
            }

            if (boolIntentMatch)
            {
                // Determine the days in the past
                // to search for High Scores
                int intDays = -1;

                #region PeriodOfTime
                EntityRecommendation PeriodOfTime;
                if (result.TryFindEntity("PeriodOfTime", out PeriodOfTime))
                {
                    switch (PeriodOfTime.Entity)
                    {
                        case "month":
                            intDays = -30;
                            break;
                        case "day":
                            intDays = -1;
                            break;
                        case "week":
                            intDays = -7;
                            break;
                        default:
                            intDays = -1;
                            break;
                    }
                }
                #endregion

                #region Days
                EntityRecommendation Days;
                if (result.TryFindEntity("Days", out Days))
                {
                    // Set Days
                    int intTempDays;
                    if (int.TryParse(Days.Entity, out intTempDays))
                    {
                        // A Number was passed
                        intDays = (Convert.ToInt32(intTempDays) * (-1));
                    }
                    else
                    {
                        // A number was not passed
                        // Call ParseEnglish Method
                        // From: http://stackoverflow.com/questions/11278081/convert-words-string-to-int
                        intTempDays = ParseEnglish(Days.Entity);
                        intDays = (Convert.ToInt32(intTempDays) * (-1));
                    }

                    // 30 days maximum
                    if (intDays > 30)
                    {
                        intDays = 30;
                    }
                }
                #endregion

                await ShowHighScores(context, intDays);
                context.Wait(this.MessageReceived);
            }
            else
            {
                // Not a match -- Start a new Game
                var objNumberGuesserDialog = new NumberGuesserDialog();
                context.Call(objNumberGuesserDialog, null);
            }
        }
        #endregion

        // Utility 

        #region private async Task ShowHighScores(IDialogContext context, int paramDays)
        private async Task ShowHighScores(IDialogContext context, int paramDays)
        {
            // Get the High Scores
            Models.BotDataEntities DB = new Models.BotDataEntities();

            // Get Yesterday
            var ParamYesterday = DateTime.Now.AddDays(paramDays);

            var HighScores = (from UserLog in DB.UserLogs
                              where UserLog.CountOfTurnsToWin != null
                              where UserLog.created > ParamYesterday
                              select UserLog)
                                .OrderBy(x => x.CountOfTurnsToWin)
                                .Take(5)
                                .ToList();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("High Scores:\n\n");

            foreach (var Score in HighScores)
            {
                sb.Append(String.Format("Score: {0} - {1} - ({2} {3})\n\n"
                    , Score.CountOfTurnsToWin
                    , Score.WinnerUserName
                    , Score.created.ToLocalTime().ToShortDateString()
                    , Score.created.ToLocalTime().ToShortTimeString()));
            }

            // Create a reply message
            var resultMessage = context.MakeMessage();
            resultMessage.Type = "message";
            resultMessage.Text = sb.ToString();

            // Send Message
            await context.PostAsync(resultMessage);
        }
        #endregion

        #region static int ParseEnglish(string number)
        // From: http://stackoverflow.com/questions/11278081/convert-words-string-to-int
        static int ParseEnglish(string number)
        {
            string[] words = number.ToLower().Split(new char[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ones = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] teens = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tens = { "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            Dictionary<string, int> modifiers = new Dictionary<string, int>() {
                { "billion", 1000000000},
                { "million", 1000000},
                { "thousand", 1000},
                { "hundred", 100}
            };

            if (number == "eleventy billion")
                return int.MaxValue; // 110,000,000,000 is out of range for an int!

            int result = 0;
            int currentResult = 0;
            int lastModifier = 1;

            foreach (string word in words)
            {
                if (modifiers.ContainsKey(word))
                {
                    lastModifier *= modifiers[word];
                }
                else
                {
                    int n;

                    if (lastModifier > 1)
                    {
                        result += currentResult * lastModifier;
                        lastModifier = 1;
                        currentResult = 0;
                    }

                    if ((n = Array.IndexOf(ones, word) + 1) > 0)
                    {
                        currentResult += n;
                    }
                    else if ((n = Array.IndexOf(teens, word) + 1) > 0)
                    {
                        currentResult += n + 10;
                    }
                    else if ((n = Array.IndexOf(tens, word) + 1) > 0)
                    {
                        currentResult += n * 10;
                    }
                    else if (word != "and")
                    {
                        throw new ApplicationException("Unrecognized word: " + word);
                    }
                }
            }

            return result + currentResult * lastModifier;
        }
        #endregion
    }
}