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
        public DateTime CreatedAt { get; set; }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
        public void Create(Game game)
        {
            Game = game;
            CreatedAt = DateTime.Now;
        }

        public void AcceptPlayerAnswer(string id)
        {
            var p = Players.FirstOrDefault(p => p.ID == id);
            if(p != null)
            {
                p.State = PlayerState.Accepted;
                p.Score += 100;
            }
        }

        public void RegisterHttpRequest(string id)
        {
            Players = Players.Where(p => p.LastRequestAt.AddSeconds(15) >= DateTime.Now).ToList();

            var player = Players.FirstOrDefault(p => p.ID == id);
            if (player != null)
                player.LastRequestAt = DateTime.Now;

            if (Game.Started && (!Players.Any(p => p.ID == Game.PainterId) || DateTime.Compare(Game.EndOfRound, DateTime.Now) <= 0))
                StartNewRound();

        }

        public void StartGame(string callerId)
        {
            Game.Started = true;
            StartNewRound();
        }

        string[] words = new string[] { "house", "car", "lightbulb", "mouse", "cat", "skyscraper", "ship", "gun", "god", "child", "school" };

        private void StartNewRound()
        {
            Game.RoundsLeft--;
            Game.EndOfRound = DateTime.Now.AddSeconds(Game.SecondsPerRound);
            Players.ForEach(p => p.State = PlayerState.Normal);
            if(Players.Count > 0)
            {
                Game.PainterId = Players[_random.Next(0, Players.Count)].ID;
                Players.FirstOrDefault(p => p.ID == Game.PainterId).State = PlayerState.Painter;
            }
            Game.CurretWord = words[_random.Next(0, words.Length)];
        }

        [FunctionName(nameof(GameEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext context)
            => context.DispatchAsync<GameEntity>();

        private static Random _random = new Random();
    }
}
