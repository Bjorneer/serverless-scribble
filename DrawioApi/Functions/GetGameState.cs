using DrawioFunctions.Entities;
using DrawioFunctions.Models;
using DrawioFunctions.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace DrawioApi
{
    public class GetGameState
    {
        [FunctionName(nameof(GetGameState))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log,
            [DurableClient] IDurableEntityClient client)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            req.Query.TryGetValue("token", out StringValues tokenQuery);

            if (tokenQuery.Count == 0)
                return new BadRequestResult();
            string token = tokenQuery.First();

            req.Query.TryGetValue("gamecode", out StringValues gamecodeQuery);

            if (gamecodeQuery.Count == 0)
                return new BadRequestObjectResult("");
            string gamecode = gamecodeQuery.First();

            var entityId = new EntityId("GameEntity", gamecode);
            var state = await client.ReadEntityStateAsync<GameEntity>(entityId);

            if(!state.EntityExists || state.EntityState.Game is null)
                return new BadRequestObjectResult("No game exists with that gamecode");

            await client.SignalEntityAsync(entityId, "RegisterHttpRequest", token);

            state = await client.ReadEntityStateAsync<GameEntity>(entityId);

            if (!state.EntityState.Players.Any(p => p.ID == token))
                return new BadRequestObjectResult("You are not present in this lobby");

            var gameState = new GameState
            {
                PlayerID = token,
                GameCode = gamecode,
                Players = state.EntityState.Players.Select(p => new SlimPlayer { UserName = p.UserName}).ToList(),
                Started = state.EntityState.Game.Started
            };

            return new OkObjectResult(state.EntityState);
        }
    }
}
