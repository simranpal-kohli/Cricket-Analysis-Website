using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cricket.Models
{
    public class PlayerInfoTestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public string PlayerName { get; set; }
        public int Leagueid { get; set; }
        public int CategoryId { get; set; }
    }
}