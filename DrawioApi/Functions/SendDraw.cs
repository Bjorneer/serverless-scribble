using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DrawioFunctions.Requests;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using DrawioFunctions.Entities;

namespace DrawioFunctions.Functions
{
    public static class SendDraw
    {
        [FunctionName("SendDraw")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log,
            [DurableClient] IDurableEntityClient client)
        {
            log.LogInformation("Recieved draw request");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DrawRequest data = JsonConvert.DeserializeObject<DrawRequest>(requestBody);


            var entityId = new EntityId("GameEntity", data.GameCode);
            var state = await client.ReadEntityStateAsync<GameEntity>(entityId);

            if (!state.EntityExists || state.EntityState.Game.PainterId != data.PlayerID)
            {
                return new BadRequestObjectResult("");
            }

            await client.SignalEntityAsync(entityId, "AddDrawObjects", data.DrawObjects);

            return new OkObjectResult("");
        }
    }
}
