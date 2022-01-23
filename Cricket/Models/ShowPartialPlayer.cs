using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Cricket.Models
{
    public class ShowPartialPlayer
    {
        public int PlayerId { get; set; }
        public string  PlayerName { get; set; }
        [DisplayName("Highest Runs")]
        public int PlayerRun { get; set; }
        [DisplayName("Highest Wickets")]
        public int PlayerWickets { get; set; }
        public int MatchId { get; set; }
        [DisplayName("Played Agaist")]
        public string TeamName{ get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}