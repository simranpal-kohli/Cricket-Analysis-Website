using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cricket.Models
{
    public class PlayerInfoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }

        public int SelectedCategoryId { get; set; }
        public List<CategoryDetailModel> CategoryCollection { get; set; }      
    }
}