using DrawioFunctions.Models;
using DrawioFunctions.Requests;
using DrawioFunctions.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DrawioApi
{
    public class CreateGame
    {
        private Random _random = new Random();

        [FunctionName(nameof(CreateGame))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [DurableClient] IDurableEntityClient client)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateGameRequest>(requestBody);

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
            var gamecode = CreateGameCode();
            var newGame = new Game
            {
                GameCode = gamecode,
                OwnerID = player.ID,
                RoundsLeft = 10,
                SecondsPerRound = 90
            };
            try
            {
                var entityId = new EntityId("GameEntity", gamecode);
                await client.SignalEntityAsync(entityId, "Create", newGame);
                await client.SignalEntityAsync(entityId, "AddPlayer", player);
            }
            catch (Exception e)
            {
                log.LogError("EXCEPTION: " + e.Message);
                return new BadRequestObjectResult($"Failed to create game with code {gamecode} and playerid {player.ID}.");
            }
            var response = new GameState
            {
                GameCode = gamecode,
                Players = new List<SlimPlayer> { new SlimPlayer { UserName = player.UserName } },
                Started = false,
                PlayerID = player.ID
            };
            return new OkObjectResult(response);
        }

        private string CreateGameCode()
        {
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                int val = _random.Next(0, 26 + 10);
                if (val < 10) code += val;
                else code += (char)('A' + val - 10);
            }
            return code;
        }
    }
}