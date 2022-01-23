using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cricket.Models
{
    public class PlayerInfo_League_CategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }

        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public int LeagueId { get; set; }
        public string LeagueName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}