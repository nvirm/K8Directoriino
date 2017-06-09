using System;
using System.Collections.Generic;

namespace K8Director.Models
{
    public partial class TeamCaptainUser
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }

        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
