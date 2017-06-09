using System;
using System.Collections.Generic;

namespace K8Director.Models
{
    public partial class Scoreboard
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public string CityName { get; set; }
        public int? StdPoints { get; set; }
        public int? GamesPlayed { get; set; }
        public int? RndWon { get; set; }
        public int? RndLose { get; set; }
        public int? Division { get; set; }
    }
}
