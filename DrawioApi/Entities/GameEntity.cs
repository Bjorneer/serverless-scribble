﻿using DrawioFunctions.Models;
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
        }

        public void StartGame(string callerId)
        {
            Game.CurretWord = "Hello";
            Game.Started = true;
        }

        [FunctionName(nameof(GameEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext context)
            => context.DispatchAsync<GameEntity>();
    }
}
