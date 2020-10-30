using DrawioFunctions.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DrawioApi
{
    public class GetGameState
    {
        private readonly CosmosClient _cosmosClient;

        private Database _database;
        private Container _container;

        public GetGameState(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;

            _database = _cosmosClient.GetDatabase(Constants.COSMOS_DB_DATABASE);
            _container = _database.GetContainer(Constants.COSMOS_DB_CONTAINER);
        }

        [FunctionName(nameof(GetGameState))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return new OkObjectResult("Successfully got gamestate");
        }
    }
}
