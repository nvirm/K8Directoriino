using System;
using System.Collections.Generic;

namespace K8Director.Models
{
    public partial class Team
    {
        public Team()
        {
            MatchTeams = new HashSet<MatchTeams>();
            TeamCaptainUser = new HashSet<TeamCaptainUser>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string CityName { get; set; }
        public string TeamName { get; set; }
        public int? CaptainUserId { get; set; }
        public int? Division { get; set; }
        public int? Eliminated { get; set; }

        public virtual ICollection<MatchTeams> MatchTeams { get; set; }
        public virtual ICollection<TeamCaptainUser> TeamCaptainUser { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
