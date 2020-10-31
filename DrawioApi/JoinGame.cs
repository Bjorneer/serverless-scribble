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
using System.Linq;
using System.Threading.Tasks;

namespace DrawioApi
{
    public class JoinGame
    {
        private readonly CosmosClient _cosmosClient;

        private Database _database;
        private Container _container;

        public JoinGame(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;

            _database = _cosmosClient.GetDatabase(Constants.COSMOS_DB_DATABASE);
            _container = _database.GetContainer(Constants.COSMOS_DB_CONTAINER);
        }

        [FunctionName(nameof(JoinGame))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<JoinGameRequest>(requestBody);

            data.UserName = data.UserName.Replace(" ", "");

            if (string.IsNullOrEmpty(data.UserName) || data.UserName.Length < 5)
                return new BadRequestObjectResult("Username is required and has to be longer than 4 characters.");

            string sql = $"SELECT TOP 1 * FROM g WHERE g.gamecode = '{data.GameCode}'";
            var query = new QueryDefinition(sql);
            FeedIterator<Game> feedIterator = _container.GetItemQueryIterator<Game>(sql);

            if (!feedIterator.HasMoreResults)
                return new BadRequestObjectResult("Invalid game code.");

            var game = (await feedIterator.ReadNextAsync()).FirstOrDefault();

            if(game == null)
                return new BadRequestObjectResult("Invalid game code.");

            if (game.Players.Any(p => p.UserName == data.UserName))
                return new BadRequestObjectResult("A player with that username already exists in this lobby");

            var player = new Player
            {
                UserName = data.UserName,
                PlayerID = Guid.NewGuid().ToString(),
                Score = 0
            };
            game.Players.Add(player);
            ItemResponse<Game> item = null;
            try
            {
                item = await _container.ReplaceItemAsync<Game>(game, game.ID, new PartitionKey(game.GameCode));
            }
            catch (Exception e)
            {
                log.LogError("EXCEPTION: " + e.Message);
                return new BadRequestObjectResult("Something went wrong with the Database connection.");
            }
            log.LogError(item.RequestCharge.ToString());
            var response = new GameState
            {
                GameCode = item.Resource.GameCode,
                Players = item.Resource.Players,
                Started = false
            };

            return new OkObjectResult(response);

        }
    }
}
