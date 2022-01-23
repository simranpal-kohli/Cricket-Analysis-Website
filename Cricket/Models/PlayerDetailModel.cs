using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cricket.Models
{
    public class PlayerDetailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerInfoId { get; set; }
        public int PlayerRun { get; set; }
        public decimal SR { get; set; }
        public int PlayerWickets { get; set; }
        public int Overs { get; set; }
        public decimal ER { get; set; }

        public int PlayerId { get; set; }
        //public string PlayerName { get; set; }

        public int MatchId { get; set; }

        public int CategoryId { get; set; }
        //public string CategoryName { get; set; }

    }
}