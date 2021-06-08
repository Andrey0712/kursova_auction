using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebGallery.Entities.Identity;

namespace WebAuction.Entities
{
    [Table("tblUserLot")]
    public class UserLot
    {
        
        public int UserId { get; set; }
        public virtual AppUser User { get; set; }
        public int LotId { get; set; }
        public virtual Lot Lot { get; set; }
    }
    
}
