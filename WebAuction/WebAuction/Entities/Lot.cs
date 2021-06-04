using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAuction.Entities
{
    [Table("tblLot")]
    public class Lot
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public int Prise { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public virtual ICollection<UserLot> UserLot { get; set; }
    }
}

