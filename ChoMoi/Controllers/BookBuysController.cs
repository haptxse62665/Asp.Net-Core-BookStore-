using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChoMoi.Api.Models;
using DemoAPI.Models;

namespace ChoMoi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookBuysController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public BookBuysController(BookStoreContext context)
        {
            _context = context;
        }

        // GET: api/BookBuys
        [HttpGet]
        public IEnumerable<BookBuy> GetBookBuy()
        {
            return _context.BookBuy;
        }

        // GET: api/BookBuys/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookBuy([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookBuy = await _context.BookBuy.FindAsync(id);

            if (bookBuy == null)
            {
                return NotFound();
            }

            return Ok(bookBuy);
        }

        // PUT: api/BookBuys/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookBuy([FromRoute] int id, [FromBody] BookBuy bookBuy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookBuy.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookBuy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookBuyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BookBuys
        [HttpPost]
        public async Task<IActionResult> PostBookBuy([FromBody] BookBuy bookBuy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BookBuy.Add(bookBuy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookBuy", new { id = bookBuy.Id }, bookBuy);
        }

        // DELETE: api/BookBuys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookBuy([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookBuy = await _context.BookBuy.FindAsync(id);
            if (bookBuy == null)
            {
                return NotFound();
            }

            _context.BookBuy.Remove(bookBuy);
            await _context.SaveChangesAsync();

            return Ok(bookBuy);
        }

        private bool BookBuyExists(int id)
        {
            return _context.BookBuy.Any(e => e.Id == id);
        }
    }
}