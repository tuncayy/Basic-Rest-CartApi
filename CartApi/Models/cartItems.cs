using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class cartItems
    {
        public int Id { get; set; }
        public String ItemtName { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
    }
}
