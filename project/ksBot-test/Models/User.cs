using System;
using System.Collections.Generic;

namespace K8Director.Models
{
    public partial class User
    {
        public User()
        {
            MatchUsers = new HashSet<MatchUsers>();
            TeamCaptainUser = new HashSet<TeamCaptainUser>();
        }

        public int Id { get; set; }
        public string DiscordId { get; set; }
        public string Username { get; set; }
        public string BtagId { get; set; }
        public string Division { get; set; }
        public int? CurrentSr { get; set; }
        public int? ApicurrentSr { get; set; }
        public string Apihero1 { get; set; }
        public string Apihero2 { get; set; }
        public string Apihero3 { get; set; }
        public double? ApikillAvg { get; set; }
        public double? ApideathAvg { get; set; }
        public double? ApihealAvg { get; set; }
        public int? TeamId { get; set; }
        public DateTime? Added { get; set; }
        public int? Attending { get; set; }
        public double? ApitimePlayed { get; set; }
        public string ApiavatarUrl { get; set; }
        public double? ApiwinRate { get; set; }

        public virtual ICollection<MatchUsers> MatchUsers { get; set; }
        public virtual ICollection<TeamCaptainUser> TeamCaptainUser { get; set; }
        public virtual Team Team { get; set; }
    }
}
