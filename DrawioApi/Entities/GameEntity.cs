using DrawioFunctions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrawioFunctions.Entities
{
    public class GameEntity
    {
        public Game Game { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();

        public void AddPlayer(Player player)
        {
            if (Players.Count < 10)
                Players.Add(player);
        }
        public void Create(Game game)
        {
            Game = game;
        }

        public void RegisterHttpRequest(string id)
        {
            Players = Players.Where(p => p.LastRequestAt.AddSeconds(15) >= DateTime.Now).ToList();

            var player= Players.FirstOrDefault(p => p.ID == id);
            if (player != null)
                player.LastRequestAt = DateTime.Now;
        }

        public void StartGame(string callerId)
        {
            if (Game.OwnerID == callerId)
                Game.Started = true;
        }

        [FunctionName(nameof(GameEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext context)
            => context.DispatchAsync<GameEntity>();
    }
}
