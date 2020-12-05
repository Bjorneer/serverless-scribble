using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Scribble.Functions.Models;
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
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var game = context.GetInput<Game>();

            string roundWord = await context.CallActivityAsync<string>("GameOrchestrator_RandomWord", null);
            string painterId = null;

            if (!game.Players.Any(p => p.ID == game.LastPainterId))
                game.LastPainterId = null;

            if (game.Players.Count == 1)
                painterId = game.Players.First().ID;
            else
                painterId = game.Players.Where(p => p.ID != game.LastPainterId).ToArray()[await context.CallActivityAsync<int>("GameOrchestrator_Random", game.Players.Count - (game.LastPainterId is null ? 0 : 1))].ID;

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

            int correct = 0;

            while (correct != game.Players.Count - 1 && game.Players.Count != 1)
            {
                var correctGuessTask = context.WaitForExternalEvent<string>("CorrectGuess");

                var finsihedTask = await Task.WhenAny(timerTask, correctGuessTask);

                if (finsihedTask == timerTask)
                    break;

                var correctPlayer = state.Players.FirstOrDefault(p => p.ID == correctGuessTask.Result && p.IsCorrect == false && p.ID != state.PainterId);
                if(correctPlayer != null)
                {
                    correct++;
                    correctPlayer.IsCorrect = true;
                    context.SetCustomStatus(state);
                }
            }
            cts.Cancel();

            context.SetCustomStatus(null);

            await context.CallActivityAsync("GameOrchestrator_EndOfRound", new Tuple<string, string>(roundWord, game.GameCode));

            if (game.RoundsLeft > 0)
            {
                game.RoundsLeft--;
                game.LastPainterId = painterId;
                game.Players.ForEach(p => p.IsCorrect = false);
                context.ContinueAsNew(game);
            }
            else
                await context.CallActivityAsync("GameOrchestrator_EndOfGame", game.GameCode);
        }

        [FunctionName("GameOrchestrator_EndOfRound")]
        public static Task EndOfRound([ActivityTrigger] Tuple<string, string> tuple,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = tuple.Item2,
                Target = "inMessage",
                Arguments = new[] { new MessageItem { User="GAME_EVENT", Message=$"ROUND END | Word was {tuple.Item1}" } }
            });
        }

        [FunctionName("GameOrchestrator_EndOfGame")]
        public static Task EndOfGame([ActivityTrigger] string groupName,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(new SignalRMessage
            {

                GroupName = groupName,
                Target = "inMessage",
                Arguments = new[] { new MessageItem { User = "GAME_EVENT", Message = $"GAME IS FINISHED" } }

            });
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

        private const int NUMBER_OF_WORDS = 2298;

        [FunctionName("GameOrchestrator_RandomWord")]
        public static string RandomWord(
            [ActivityTrigger] string t,
            [Table("ScribbleWords", Connection = "AzureWebJobsStorage")] CloudTable cloudTable)
        {
            int rand = _random.Next(1, NUMBER_OF_WORDS + 1);
            TableOperation getItem = TableOperation.Retrieve<WordEntity>("Words", rand.ToString());
            var query = cloudTable.Execute(getItem);
            if (query.Result == null)
                return "TEST";
            return ((WordEntity)query.Result).Word;
        }

        [FunctionName("GameOrchestrator_Random")]
        public static int Random([ActivityTrigger] int max)
        {
            return _random.Next(0, max);

        }



    }
}