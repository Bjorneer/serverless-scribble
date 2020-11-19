using DrawioFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

            string roundWord = "Hello"; // make api call to some where that creates random noun

            var game = context.GetInput<Game>();

            var cts = new CancellationTokenSource();
            var timerTask = context.CreateTimer(DateTime.Now.AddSeconds(game.SecondsPerRound), cts.Token);

            while (true)
            {
                var drawEvent = context.WaitForExternalEvent<List<DrawObject>>("Draw");
                var guessEvent = context.WaitForExternalEvent<Tuple<string,string>>("Guess");

                var task = await Task.WhenAny( drawEvent, timerTask, guessEvent);

                if (task == drawEvent)
                {
                    await context.CallActivityAsync("GameOrchestrator_Draw", guessEvent.Result);

                    drawEvent = context.WaitForExternalEvent<List<DrawObject>>("Draw");
                }
                else if (task == guessEvent)
                {
                    if (guessEvent.Result.Item2.ToLower() == roundWord.ToLower())
                    {
                        await context.CallActivityAsync("GameOrchestrator_CorrectGuess", guessEvent.Result.Item1);
                    }

                    guessEvent = context.WaitForExternalEvent<Tuple<string, string>>("Guess");
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