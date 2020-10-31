using DrawioFunctions.Helpers;
using DrawioFunctions.Models;
using DrawioFunctions.Requests;
using DrawioFunctions.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DrawioApi
{
    public class CreateGame
    {
        private readonly CosmosClient _cosmosClient;

        private Database _database;
        private Container _container;
        private Random _random = new Random();

        public CreateGame(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;

            _database = _cosmosClient.GetDatabase(Constants.COSMOS_DB_DATABASE);
            _container = _database.GetContainer(Constants.COSMOS_DB_CONTAINER);
        }

        [FunctionName(nameof(CreateGame))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateGameRequest>(requestBody);

            data.UserName = data.UserName.Replace(" ", "");

            if (string.IsNullOrEmpty(data.UserName) || data.UserName.Length < 5)
                return new BadRequestObjectResult("Username is required and has to be longer than 4 characters.");

            var player = new Player
            {
                UserName = data.UserName,
                PlayerID = Guid.NewGuid().ToString(),
                Score = 0
            };
            var gamecode = CreateGameCode();
            var newGame = new Game
            {
                ID = Guid.NewGuid().ToString(),
                Players = new System.Collections.Generic.List<Player>(new Player[]{ player }),
                GameCode = gamecode,
                OwnerID = player.PlayerID,
                Key = gamecode
            };
            ItemResponse<Game> item = null;
            try
            {
                item = await _container.CreateItemAsync<Game>(newGame);
            }
            catch(Exception e)
            {
                log.LogError("EXCEPTION: " + e.Message);
                return new BadRequestObjectResult("Something went wrong with the Database connection.");
            }
            var response = new GameState
            {
                GameCode = gamecode,
                Players = item.Resource.Players,
                Started = false
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
