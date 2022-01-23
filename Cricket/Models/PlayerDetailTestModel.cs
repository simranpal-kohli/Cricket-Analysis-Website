using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cricket.Models
{
    public class PlayerDetailTestModel
    {
        public int PlayerInfoId { get; set; }
        public int PlayerRun { get; set; }
        public decimal SR { get; set; }
        public int PlayerWickets { get; set; }
        public decimal ER { get; set; }
        public int Overs { get; set; }

        public int PlayerId { get; set; }
        public string PlayerName { get; set; }

        public int MatchId { get; set; }
        public int TeamAId { get; set; }
        public string TeamAName { get; set; }
        public int TeamBId { get; set; }
        public string TeamBName { get; set; }
        public string Venue { get; set; }
        public DateTime MatchDate { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
       
    }
}