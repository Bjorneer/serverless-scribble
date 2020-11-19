using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DrawioFunctions.Models;
using DrawioFunctions.Requests;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
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

                    await context.CallActivityAsync("LobbyOrchestrator_Join", new JoinRequest { UserName = joinGameEventTask.Result.UserName, GameCode = context.InstanceId });

                    joinGameEventTask = context.WaitForExternalEvent<Player>("Join");
                }
                else
                {
                    break;
                }
            }

            context.StartNewOrchestration(nameof(GameOrchestrator), game, "g" + context.InstanceId);

            await context.CallActivityAsync("LobbyOrchestrator_Start", context.InstanceId);

            return game;
        }

        [FunctionName("LobbyOrchestrator_Join")]
        public static Task Join(
            [ActivityTrigger] JoinRequest req,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = req.GameCode,
                Target = "userJoined",
                Arguments =  new[] { req.UserName }
            });
        }

        [FunctionName("LobbyOrchestrator_Start")]
        public static Task Start(
            [ActivityTrigger] string instanceId,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            return signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = instanceId,
                Target = "userJoined",
                Arguments = null
            });
        }
    }
}