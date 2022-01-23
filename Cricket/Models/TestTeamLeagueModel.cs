using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cricket.Models
{
    public class TestTeamLeagueModel
    {
        public List<TeamDetailModel> testteamdetail { get; set; }
        //public List<TestModel> testleaguedetail { get; set; }
        [Display(Name ="Select League"),Required(ErrorMessage ="Please select League")]
        public int SelectedLeagueId { get; set; }
        public List<LeagueDetailModel> LeagueCollection { get; set; }
        public string SelectedLeagueName { get; set; }
    }
}