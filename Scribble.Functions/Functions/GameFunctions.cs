using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Scribble.Functions.Models;
using Scribble.Functions.Requests;
using Scribble.Functions.Responses;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Scribble.Functions.Functions
{
    public static class GameFunctions
    {
        [FunctionName("draw")]
        public static async Task<ActionResult> Draw(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<DrawRequest>(requestBody);


            if (data.GameCode == null)
                return new BadRequestResult();

            var status = await client.GetStatusAsync("g" + data.GameCode);

            if (status == null)
                return new NotFoundResult();

            if (status.RuntimeStatus != OrchestrationRuntimeStatus.Running)
                return new BadRequestResult();

            var state = status.CustomStatus.ToObject<RoundState>();

            if (state.PainterId != data.PlayerID)
                return new BadRequestResult();

            await signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = data.GameCode,
                Target = "draw",
                Arguments = new[] { data.DrawObjects }
            });

            return new OkResult();
        }

        [FunctionName("guess")]
        public static async Task<IActionResult> Guess(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient client,
            [SignalR(HubName = "game")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<GuessRequest>(requestBody);

            if (data.GameCode == null ||data.Guess == null || data.Guess.Length > 35)
                return new BadRequestResult();

            var status = await client.GetStatusAsync("g" + data.GameCode);

            if (status == null)
                return new NotFoundResult();

            if (status.RuntimeStatus != OrchestrationRuntimeStatus.Running)
                return new BadRequestResult();

            var state = status.CustomStatus.ToObject<RoundState>();

            if (!state.Players.Any(p => p.ID == data.PlayerID) || state.PainterId == data.PlayerID)
                return new BadRequestResult();

            if (state.Word.ToLower() != data.Guess.ToLower())
            {
                await signalRMessages.AddAsync(new SignalRMessage
                {
                    GroupName = data.GameCode,
                    Target = "inMessage",
                    Arguments = new[] { new MessageItem {User=state.Players.First(p =>p.ID == data.PlayerID).UserName, Message = data.Guess } }
                });
                return new BadRequestResult();
            }
            await signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = data.GameCode,
                Target = "guessCorrect",
                Arguments = new[] { state.Players.First(p => p.ID == data.PlayerID).UserName }
            });
            await signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = data.GameCode,
                Target = "inMessage",
                Arguments = new[] { new MessageItem { User = "GAME_EVENT", Message = $"{state.Players.First(p => p.ID == data.PlayerID).UserName} guessed correctly"} }
            });


            return new OkResult();
        }
    }
}
