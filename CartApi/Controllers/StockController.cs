using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CartApi.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private ItemContext _context;

        public StockController(ItemContext context)
        {
            _context = context;

            if (_context.Stocks.Count() == 0)
            {
                _context.Stocks.Add(new stockItems { Name = "flower A", Price = 10, StockCount = 10 });
                _context.Stocks.Add(new stockItems { Name = "flower B", Price = 4, StockCount = 11 });
                _context.Stocks.Add(new stockItems { Name = "flower C", Price = 6, StockCount = 12 });
                _context.Stocks.Add(new stockItems { Name = "Gift A", Price = 24, StockCount = 3 });
                _context.Stocks.Add(new stockItems { Name = "Gift B", Price = 34, StockCount = 14 });
                _context.Stocks.Add(new stockItems { Name = "Gift C", Price = 12, StockCount = 15 });

                _context.SaveChanges();
               
            }
        }

        //GET: api/stock
        [HttpGet]
        public async Task<ActionResult<IEnumerable<stockItems>>> GetItems()
        {
            return await _context.Stocks.ToListAsync();
        }
    }
}
