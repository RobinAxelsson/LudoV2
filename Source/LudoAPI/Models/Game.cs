using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LudoAPI.DataAccess;

namespace LudoAPI.Models
{
    public record Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        ICollection<Player> Players { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ModelEnum.GameStatus GameStatus { get; set; }
    }
}