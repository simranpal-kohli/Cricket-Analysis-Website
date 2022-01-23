using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Cricket.Models
{
    public class DetailModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }

        public int MatchesPlayed { get; set; }
        public int CategoryId { get; set; }
        public int PlayerInfoId { get; set; }
        [DisplayName("Highest Score")]
        public int MaxRuns { get; set; }
        [DisplayName("Avg SR")]
        public decimal AvgSR { get; set; }
        [DisplayName("Highest Wickets")]
        public int MaxWickets { get; set; }
        [DisplayName("Avg ER")]
        public decimal AvgER { get; set; }
        public string Review { get; set; }
    }
}