using System.Collections.Generic;
using LudoAPI.DataAccess;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LudoServiceLib.GameEngine.Models
{
    public record Player
    {
        public int AccountId { get; set; }
        public bool NextToThrow { get; set; }
        public int Result { get; set; }
        public ICollection<Coordinate> Pawns { get; set; }
        public GameEnum.TeamColor Color { get; set; }
        public ModelEnum.PlayerType Type { get; set; }
    }

    public record Coordinate
    {
        int X { get; set; }
        int Y { get; set; }
    }
}