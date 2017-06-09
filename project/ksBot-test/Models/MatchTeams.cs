using System;
using System.Collections.Generic;

namespace K8Director.Models
{
    public partial class MatchTeams
    {
        public int TeamId { get; set; }
        public int MatchId { get; set; }
        public int? TeamId1Points { get; set; }
        public int? TeamId2Points { get; set; }
        public string TeamId1DraftUrl { get; set; }
        public string TeamId2DraftUrl { get; set; }
        public int? TeamId1CaptainUserId { get; set; }
        public int? TeamId2CaptainUserId { get; set; }

        public virtual Match Match { get; set; }
        public virtual Team Team { get; set; }
    }
}
