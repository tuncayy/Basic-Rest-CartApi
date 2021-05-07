using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CartApi.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CartApi.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ItemContext _context;
        public CartController(ItemContext context)
        {
            _context = context;

        }

        //GET: api/cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<cartItems>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        //GET: api/cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<cartItems>> GetItem(int id)
        {
            var Item = await _context.Items.FindAsync(id);
            if (Item == null)
            {
                return NotFound();
            }
            return Item;
        }

        //POST: api/cart
        [HttpPost]
        public async Task<ActionResult<cartItems>> PostItem(int id, int quantity)
        {
            var StockItem = await _context.Stocks.FindAsync(id);
            var CartItem = await _context.Items.FindAsync(id);
            if (StockItem == null)
            {
                return NotFound("enter a valid Id");
            }
            if (StockItem.StockCount >= quantity && CartItem == null)
            {
                var item = new cartItems
                {
                    Id = id,
                    ItemtName = StockItem.Name,
                    Quantity = quantity,
                    TotalPrice = (StockItem.Price * quantity)
                };
                _context.Items.Add(item);
                StockItem.StockCount -= quantity;

                await _context.SaveChangesAsync();
                var items = await _context.Items.ToListAsync();
                return CreatedAtAction(nameof(GetItems), items);
            }
            else if (StockItem.StockCount >= quantity && CartItem != null)
            {
                CartItem.Quantity += quantity;
                CartItem.TotalPrice += (quantity * StockItem.Price);
                StockItem.StockCount -= quantity;

                await _context.SaveChangesAsync();
                var items = await _context.Items.ToListAsync();
                return CreatedAtAction(nameof(GetItems), items);
            }
            else
            {
                return NotFound("only " + StockItem.StockCount + " left in stocks");
            }

        }

        // DELETE: api/cart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var stockItem = await _context.Stocks.FindAsync(id);
            stockItem.StockCount += item.Quantity;
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            var items = await _context.Items.ToListAsync();
            return CreatedAtAction(nameof(GetItems), items);
        }

        // PUT: api/item/
        [HttpPut]
        public async Task<IActionResult> UpdateItem(int id, int quantity)
        {

            var item = await _context.Items.FindAsync(id);
            var stockItem = await _context.Stocks.FindAsync(id);
            if (item == null)
            {
                return NotFound("this item is not exist on your cart");
            }
            if (quantity == 0)
            {
                stockItem.StockCount += item.Quantity;
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
                return NotFound("item deleted from cart");
            }
            else
            {
                if (item.Quantity + stockItem.StockCount < quantity)
                {
                    return NotFound("only " + stockItem.StockCount + " left in stocks");
                }
                if (item.Quantity > quantity)
                {
                    item.TotalPrice -= ((item.Quantity - quantity) * stockItem.Price);
                    stockItem.StockCount += (item.Quantity - quantity);
                }
                else if (item.Quantity < quantity)
                {
                    item.TotalPrice += ((quantity - item.Quantity) * stockItem.Price);
                    stockItem.StockCount -= (quantity - item.Quantity);
                }
                item.Quantity = quantity;
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
            }



        }
    }
}
