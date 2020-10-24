using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScriblioClone.App.Cosmos;
using ScriblioClone.App.Models;

namespace ScriblioClone.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private ICosmosDbService _cosmosDbService;

        public GameController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost]
        public async Task<ActionResult<CreateGame>> CreateGame(CreateGame game)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult<CreateGame>> JoinGame(CreateGame game)
        {
            throw new NotImplementedException();
        }
    }
}
