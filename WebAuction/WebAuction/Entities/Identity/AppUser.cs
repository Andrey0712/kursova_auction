using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAuction.Entities;


namespace WebGallery.Entities.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<UserLot> UserLot { get; set; }
    }
}
