using DrawioFunctions.Models;
using DrawioFunctions.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
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
        private static Random _random = new Random();


        [FunctionName("GameOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context) // consider moving events except timer out of orchestrator to stop it from starting all the time
        {

            var game = context.GetInput<Game>();

            string[] _words = "Arrow,Panda,Ribs,Banana,Oil,Belt,Cheese,Desk,Legs,Air Conditioner,Shark,Rasp Berries,Bear,Hands,Fan,Chairs,Bird,Cap,Suit,Ostrich,Skyscraper".Split(',');

            string roundWord = _words[await context.CallActivityAsync<int>("GameOrchestrator_Random", _words.Length)];
            string painterId = game.Players[await context.CallActivityAsync<int>("GameOrchestrator_Random", game.Players.Count)].ID;

            await context.CallActivityAsync("GameOrchestrator_StartNewRound", new Tuple<string,string>(game.Players.First(p => p.ID == painterId).UserName, game.GameCode));

            await context.CallActivityAsync("GameOrchestrator_MakePainter", new Tuple<string, string>(roundWord, painterId));

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
                        await context.CallActivityAsync("GameOrchestrator_Draw", new Tuple<List<DrawObject>, string>(drawEvent.Result.DrawObjects, game.GameCode));

                    drawEvent = context.WaitForExternalEvent<DrawRequest>("Draw");
                }
                else if (task == guessEvent)
                {
                    if (guessEvent.Result.Guess.ToLower() == roundWord.ToLower() && painterId != guessEvent.Result.PlayerID && game.Players.Any(p => p.ID == guessEvent.Result.PlayerID))
                        await context.CallActivityAsync("GameOrchestrator_CorrectGuess", new Tuple<string, string>(game.Players.First(p => p.ID == guessEvent.Result.PlayerID).UserName, game.GameCode));

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
        public static Task StartNewRound([ActivityTrigger] Tuple<string, string> tuple,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = tuple.Item2,
                Target = "newRound",
                Arguments = new[] { tuple.Item1 }
            });
        }

        [FunctionName("GameOrchestrator_MakePainter")]
        public static Task MakePainter([ActivityTrigger] Tuple<string, string> tuple,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(new SignalRMessage
            {
                UserId = tuple.Item2,
                Target = "makePainter",
                Arguments = new[] { tuple.Item1}
            });
        }

        [FunctionName("GameOrchestrator_CorrectGuess")]
        public static Task CorrectGuess([ActivityTrigger] Tuple<string, string> tuple,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = tuple.Item2,
                Target = "guessCorrect",
                Arguments = new[] { tuple.Item1 }
            });
        }

        [FunctionName("GameOrchestrator_Draw")]

        public static Task Draw([ActivityTrigger] Tuple<List<DrawObject>, string> tuple,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = tuple.Item2,
                Target = "draw",
                Arguments = new[] { tuple.Item1 }
            });
        }

        [FunctionName("GameOrchestrator_Random")]

        public static int Random([ActivityTrigger] int max)
        {
            return _random.Next(0, max);
        }



    }
}