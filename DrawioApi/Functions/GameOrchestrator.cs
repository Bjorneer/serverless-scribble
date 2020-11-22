using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Scribble.Functions.Models;
using System;
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
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var game = context.GetInput<Game>();

            string[] _words = "Arrow,Panda,Ribs,Banana,Oil,Belt,Cheese,Desk,Legs,Air Conditioner,Shark,Raspberries,Bear,Hands,Fan,Chairs,Bird,Cap,Suit,Ostrich,Skyscraper".Split(',');

            string roundWord = _words[await context.CallActivityAsync<int>("GameOrchestrator_Random", _words.Length)];
            string painterId = game.Players[await context.CallActivityAsync<int>("GameOrchestrator_Random", game.Players.Count)].ID;

            var state = new RoundState
            {
                PainterId = painterId,
                Players = game.Players,
                Word = roundWord
            };

            context.SetCustomStatus(state);

            await context.CallActivityAsync("GameOrchestrator_StartNewRound", new Tuple<string, string>(game.Players.First(p => p.ID == painterId).UserName, game.GameCode));

            await context.CallActivityAsync("GameOrchestrator_MakePainter", new Tuple<string, string>(roundWord, painterId));

            var cts = new CancellationTokenSource();
            var timerTask = context.CreateTimer(context.CurrentUtcDateTime.Add(TimeSpan.FromSeconds(game.SecondsPerRound)), cts.Token);

            await timerTask;

            if (game.RoundsLeft > 0)
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