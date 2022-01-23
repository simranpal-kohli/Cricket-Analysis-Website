using Cricket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cricket.Models
{
    public class TeamDetailLeagueDetailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        [Column(TypeName = "varbinary")]
        public Byte[] Flag { get; set; }

        [Display(Name ="League")]
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        //[NotMapped]
        //public List<LeagueDetailModel> LeagueCollection { get; set; }
       
                  
        //public IEnumerable<SelectListItem> IEnumerableLeague
        //{
        //    get { return new SelectList(LeagueCollection, "LeagueId", "LeagueName"); }
        //}
    }
}