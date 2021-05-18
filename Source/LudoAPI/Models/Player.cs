using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LudoAPI.DataAccess;
using LudoGame.GameEngine;

namespace LudoAPI.Models
{
    public record Player
    {
        [Key]
        public int GameID { get; set; }
        public int AccountId { get; set; }
        public bool NextToThrow { get; set; }
        public int Result { get; set; }
        public ICollection<Coordinate> PawnCoords { get; set; }
        public GameEnum.TeamColor Color { get; set; }
        public ModelEnum.PlayerType Type { get; set; }
    }

    public record Coordinate
    {
        int X { get; set; }
        int Y { get; set; }
    }
}