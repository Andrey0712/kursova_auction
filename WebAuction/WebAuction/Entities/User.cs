using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAuction.Entities
{
    [Table("tblUser")]

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(150)]
        public string Surname { get; set; }

        [Required, StringLength(100)]
        public string Login { get; set; }

        [Required, StringLength(120)]
        public string Password { get; set; }
        public virtual ICollection<UserLot> UserLot { get; set; }


    }
}
