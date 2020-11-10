using DrawioFunctions.Entities;
using DrawioFunctions.Models;
using DrawioFunctions.Requests;
using DrawioFunctions.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DrawioApi
{
    public class JoinGame
    {
        [FunctionName(nameof(JoinGame))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [DurableClient] IDurableEntityClient client)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<JoinGameRequest>(requestBody);

            data.UserName = data.UserName.Replace(" ", "");

            if (string.IsNullOrEmpty(data.UserName) || data.UserName.Length < 5)
                return new BadRequestObjectResult("Username is required and has to be longer than 4 characters.");

            var player = new Player
            {
                UserName = data.UserName,
                ID = Guid.NewGuid().ToString(),
                Score = 0,
                LastRequestAt = DateTime.Now
            };
            var players = new List<Player>();
            try
            {
                var entityId = new EntityId("GameEntity", data.GameCode);
                var state = await client.ReadEntityStateAsync<GameEntity>(entityId);
                if (!state.EntityExists || state.EntityState.Game == null)
                    return new BadRequestObjectResult("No such game exists");

                if (state.EntityState.Players.Count() >= 10)
                    return new BadRequestErrorMessageResult("Lobby is already full");

                if (state.EntityState.Game.Started)
                    return new BadRequestErrorMessageResult("Game has already started");

                if(state.EntityState.Players.Any(t => t.UserName == player.UserName))
                    return new BadRequestObjectResult("Your username is not unique to this lobby");

                await client.SignalEntityAsync(entityId, "AddPlayer", player);

                players = state.EntityState.Players;
                players.Add(player);
            }
            catch (Exception e)
            {
                log.LogError($"EXCEPTION: {e.Message}");
                return new BadRequestObjectResult("Unable to join game");
            }
            var response = new GameState
            {
                GameCode = data.GameCode,
                Players = players.Select(p => new SlimPlayer { UserName = p.UserName }).ToList(),
                Started = false,
                PlayerID = player.ID
            };

            return new OkObjectResult(response);

        }
    }
}
