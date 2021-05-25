using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LudoDataAccess.Database;
using LudoGame.GameEngine;

namespace LudoDataAccess.Models
{
    public record Player
    {
        [Key]
        public int PlayerId { get; set; }
        public Game Game { get; set; }
        public int AccountId { get; set; }
        public bool NextToThrow { get; set; }
        public int Result { get; set; }
        public ICollection<Coordinates> PawnCoords { get; set; }
        public GameEnum.TeamColor Color { get; set; }
        public ModelEnum.PlayerType Type { get; set; }
    }

    public record Coordinates
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}