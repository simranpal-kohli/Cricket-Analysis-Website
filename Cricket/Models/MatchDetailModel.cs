using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cricket.Models
{
    public class MatchDetailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatchId { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        //public string TeamName { get; set; }
        public string TeamAName { get; set; }
        public string TeamBName { get; set; }
        public string Venue { get; set; }
        public DateTime MatchDate { get; set; }
        public int LeagueId { get; set; }
    }
}