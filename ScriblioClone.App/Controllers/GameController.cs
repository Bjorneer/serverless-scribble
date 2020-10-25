using Microsoft.AspNetCore.Mvc;
using ScriblioClone.App.Cosmos;
using ScriblioClone.App.Models;
using ScriblioClone.App.Requests;
using ScriblioClone.App.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [ActionName("CreateGame")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GameLobbyState>> CreateGameAsync(CreateGame game)
        {
            if (game.OwnerUserName != null && game.OwnerUserName.Length < 6)
                return null;

            var users = new List<GameUser>();
            users.Add(new GameUser
            {
                UserName = game.OwnerUserName
            });
            GameState gameState = new GameState
            {
                SecondsToDraw = 90,
                GameCode = CreateGameCode(),
                RoundsLeft = 10,
                Users = new List<GameUser>()
            };
            await _cosmosDbService.CraeteGameAsync(gameState);

            return new GameLobbyState
            {
                GameCode = gameState.GameCode,
                SecondsToDraw = gameState.SecondsToDraw,
                LobbyOwnerUserName = game.OwnerUserName,
                Rounds = gameState.RoundsLeft
            };
        }

        [ActionName("StartGame")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<GameStateResponse>> StartGameAsync(StartGame game)
        {
            var gameState = await _cosmosDbService.GetGameByOwnerIdAsync(game.OwnerId);

            if (gameState == null)
                throw new ArgumentException($"Could not find game with status InLobby and OwnerId {game.OwnerId}");


            return new GameStateResponse { };
        }

        private string CreateGameCode()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
