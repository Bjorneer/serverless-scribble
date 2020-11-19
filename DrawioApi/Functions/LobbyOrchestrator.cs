using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DrawioFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Scribble.Functions.Functions
{
    public static class LobbyOrchestrator
    {
        [FunctionName("LobbyOrchestrator")]
        public static async Task<Game> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var game = context.GetInput<Game>();
            context.SetCustomStatus(game);

            var startGameEventTask = context.WaitForExternalEvent("Start");

            var joinGameEventTask = context.WaitForExternalEvent<Player>("Join");
            
            while (true)
            {
                var finishedTask = await Task.WhenAny(joinGameEventTask, startGameEventTask);
                if (joinGameEventTask == finishedTask)
                {
                    game.Players.Add(joinGameEventTask.Result);
                    context.SetCustomStatus(game);

                    await context.CallActivityAsync("LobbyOrchestrator_Join", joinGameEventTask.Result);

                    joinGameEventTask = context.WaitForExternalEvent<Player>("Join");
                }
                else
                {
                    break;
                }
            }

            context.StartNewOrchestration(nameof(GameOrchestrator), game, "s" + context.InstanceId);

            await context.CallActivityAsync("LobbyOrchestrator_Start", null);

            return game;
        }

        [FunctionName("LobbyOrchestrator_Join")]
        public static void Join(
            [ActivityTrigger] Player name,
            ILogger log)
        {
            //Send out signal to all clients that user joined/might move some entity logic here
        }

        [FunctionName("LobbyOrchestrator_Start")]
        public static void Start(
            [ActivityTrigger] string input,
            ILogger log)
        {
            //Send out signal to all clients that game has started/might be good to move entity logic here
        }
    }
}