using DrawioFunctions.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace DrawioFunctions
{
    public class StartGame
    {
        [FunctionName(nameof(StartGame))]
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
            await client.SignalEntityAsync(entityId, "StartGame", token);

            var entity = await client.ReadEntityStateAsync<GameEntity>(entityId);
            if (!entity.EntityExists || entity.EntityState.Game == null)
                return new BadRequestObjectResult("No such game exists");

            if (!entity.EntityState.Game.Started)
                return new BadRequestObjectResult("You are not the owner of this lobby"); 
            
            return new OkObjectResult("Game has been started");
        }
    }
}
