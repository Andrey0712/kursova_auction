using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAuction.Entities
{
    [Table("tblUserLot")]
    public class UserLot
    {
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int LotId { get; set; }
        public virtual Lot Lot { get; set; }
    }
    
}
