using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cricket.Models
{
    public class MatchDetail_PlayerDetail_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MatchId { get; set; }
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public string Venue { get; set; }
        public DateTime MatchDate { get; set; }
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }

        public int SelectedTeamId { get; set; }
        public List<TeamDetailModel> TeamCollection { get; set; }

        public List<PlayerNameIdModel> PlayerCollection { get; set; }

        public IList<PlayerScoringRecordsModel> PlayerScoringCollection { get; set; }
        

    }
}