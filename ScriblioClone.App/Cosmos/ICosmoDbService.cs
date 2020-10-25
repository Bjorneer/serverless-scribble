using ScriblioClone.App.Models;
using ScriblioClone.App.Requests;
using ScriblioClone.App.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriblioClone.App.Cosmos
{
    public interface ICosmosDbService
    {
        Task CraeteGameAsync(GameState game);
        Task<GameState> GetGameByOwnerIdAsync(string ownerId);
    }
}
