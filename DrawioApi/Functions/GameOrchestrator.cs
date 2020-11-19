using DrawioFunctions.Models;
using DrawioFunctions.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Scribble.Functions.Functions
{
    public static class GameOrchestrator
    {
        [FunctionName("GameOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            await context.CallActivityAsync("GameOrchestrator_StartNewRound", null);

            var game = context.GetInput<Game>();

            string roundWord = "Test"; // make some api call to that creates random noun
            string painterId = game.Players.FirstOrDefault()?.ID; // take random player instead of first


            var cts = new CancellationTokenSource();
            var timerTask = context.CreateTimer(context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(game.SecondsPerRound)), cts.Token);

            while (true)
            {
                var drawEvent = context.WaitForExternalEvent<DrawRequest>("Draw");
                var guessEvent = context.WaitForExternalEvent<GuessRequest>("Guess");

                var task = await Task.WhenAny(drawEvent, timerTask, guessEvent);

                if (task == drawEvent)
                {
                    if (painterId == drawEvent.Result.PlayerID)
                        await context.CallActivityAsync("GameOrchestrator_Draw", drawEvent.Result.DrawObjects);

                    drawEvent = context.WaitForExternalEvent<DrawRequest>("Draw");
                }
                else if (task == guessEvent)
                {
                    if (guessEvent.Result.Guess.ToLower() == roundWord.ToLower() && painterId != guessEvent.Result.PlayerID && game.Players.Any(p => p.ID == guessEvent.Result.PlayerID))
                        await context.CallActivityAsync("GameOrchestrator_CorrectGuess", game.Players.First(p => p.ID == guessEvent.Result.PlayerID).UserName);

                    guessEvent = context.WaitForExternalEvent<GuessRequest>("Guess");
                }
                else
                {
                    break;
                }
            }
            if(game.RoundsLeft > 0)
            {
                game.RoundsLeft--;
                context.ContinueAsNew(game);
            }
        }

        [FunctionName("GameOrchestrator_StartNewRound")]
        public static void StartNewRound([ActivityTrigger] string name, ILogger log)
        {
        }

        [FunctionName("GameOrchestrator_CorrectGuess")]
        public static void CorrectGuess([ActivityTrigger] string name, ILogger log)
        {
        }

        [FunctionName("GameOrchestrator_Draw")]
        public static void Draw([ActivityTrigger] List<DrawObject> objects, ILogger log)
        {

        }
    }
}