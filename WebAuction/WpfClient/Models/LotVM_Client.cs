using System;
using System.Collections.Generic;
using System.Text;

namespace WpfClient.Models
{
    class LotVM_Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Prise { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime End { get; set; }
        public DateTime Begin { get; set; }
    }
}
