using System;
using System.Collections.Generic;

namespace K8Director.Models
{
    public partial class Match
    {
        public Match()
        {
            MatchTeams = new HashSet<MatchTeams>();
            MatchUsers = new HashSet<MatchUsers>();
        }

        public int Id { get; set; }
        public string Matchcode { get; set; }
        public int? Team1Id { get; set; }
        public int? Team2Id { get; set; }
        public string ObsDraftUrl { get; set; }
        public string ManageDraftUrl { get; set; }
        public int? WinnerTeamId { get; set; }
        public bool? Closed { get; set; }
        public DateTime? Added { get; set; }
        public bool? Started { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? Projectedstarttime { get; set; }
        public int? Team1Division { get; set; }
        public int? Team2Division { get; set; }

        public virtual ICollection<MatchTeams> MatchTeams { get; set; }
        public virtual ICollection<MatchUsers> MatchUsers { get; set; }
    }
}
