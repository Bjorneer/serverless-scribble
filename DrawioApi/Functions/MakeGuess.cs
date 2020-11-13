using DrawioFunctions.Entities;
using DrawioFunctions.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DrawioFunctions
{
    public class MakeGuess
    {
        [FunctionName(nameof(MakeGuess))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log, [DurableClient] IDurableEntityClient client)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var guessReq = JsonConvert.DeserializeObject<MakeGuessRequest>(requestBody);

            var entityID = new EntityId("GameEntity", guessReq.GameCode);
            var game = await client.ReadEntityStateAsync<GameEntity>(entityID);

            if (!game.EntityExists || game.EntityState.Game == null)
                return new BadRequestResult();

            if (!game.EntityState.Players.Any(p => p.ID == guessReq.PlayerID && p.State == Models.PlayerState.Normal))
                return new BadRequestResult();

            if (game.EntityState.Game.CurretWord != guessReq.Guess)
                return new BadRequestObjectResult("Incorrect answer");

            await client.SignalEntityAsync(entityID, "AcceptPlayerAnswer", guessReq.PlayerID);

            return new OkResult();
        }
    }
}
