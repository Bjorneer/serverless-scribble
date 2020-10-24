using Microsoft.Azure.Cosmos;
using ScriblioClone.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriblioClone.App.Cosmos
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task CraeteGameAsync(GameModel game)
        {
            await this._container.CreateItemAsync<GameModel>(game, new PartitionKey(game.Id));
        }



    }
}
