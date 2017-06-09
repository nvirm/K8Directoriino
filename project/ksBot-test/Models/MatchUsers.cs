using System;
using System.Collections.Generic;

namespace K8Director.Models
{
    public partial class MatchUsers
    {
        public int UserId { get; set; }
        public int MatchId { get; set; }

        public virtual Match Match { get; set; }
        public virtual User User { get; set; }
    }
}
