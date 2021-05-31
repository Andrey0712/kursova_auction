using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAuction.Model
{
    public class LotVM
    {
        public string Name { get; set; }
        public int Prise { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
