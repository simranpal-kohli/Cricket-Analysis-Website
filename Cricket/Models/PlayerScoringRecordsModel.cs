using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cricket.Models
{
    public class PlayerScoringRecordsModel
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerInfoId { get; set; }
        public int PlayerRun { get; set; }
        public Decimal SR { get; set; }
        public int PlayerWicket { get; set; }
        public int Overs { get; set; }
        public Decimal ER { get; set; }
        public string Review { get; set; }

        public int PlayerId { get; set; }
        public String PlayerName { get; set; }
        public int CategoryId { get; set; }
    }
}