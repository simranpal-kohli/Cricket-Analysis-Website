using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cricket.Models
{
    public class MatchPlayerRecordDisplay
    {
        public List<PlayerInfoTestModel> PlyerInfoModel { get; set; }
        public List<PlayerDetailTestModel> PlayerDetailModel { get; set; }
        public List<MatchDetailModel> MatchDetailModel { get; set; }
        public List<TeamDetail> TeamDetailModel { get; set; }
    }
}