using Microsoft.Azure.Cosmos;
using ScriblioClone.App.Models;
using ScriblioClone.App.Requests;
using ScriblioClone.App.Responses;
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

        public async Task CraeteGameAsync(GameState game)
        {
            await _container.CreateItemAsync(game);
        }

        public async Task<GameState> GetGameByOwnerIdAsync(string ownerId)
        {
            return await _container.ReadItemAsync<GameState>(ownerId, new PartitionKey("/id"));
        }
    }
}
