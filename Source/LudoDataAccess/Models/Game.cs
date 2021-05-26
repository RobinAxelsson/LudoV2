using System;
using System.ComponentModel.DataAnnotations;
using LudoDataAccess.Database;

namespace LudoDataAccess.Models
{
    public record Game
    {
        public Game() { }

        public Game(string id)
        {
            GameId = id;
        }
        [Key]
        public string GameId { get; set; }
        public string Url { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ModelEnum.GameStatus GameStatus { get; set; }
    }
}