using ScriblioClone.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriblioClone.App.Cosmos
{
    public interface ICosmosDbService
    {
        Task CraeteGameAsync(GameModel game);
    }
}
