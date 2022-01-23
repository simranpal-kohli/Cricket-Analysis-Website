using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cricket.Models
{
    public class TestModel
    {
        public int SelectedLeagueId { get; set; }
        public List<LeagueDetailModel> LeagueCollection { get; set; }
    }
}