using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApi.Models
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options)
            : base(options)
        {

        }
        public DbSet<cartItems> Items { get; set; }
        public DbSet<stockItems> Stocks { get; set; }
    }
}
