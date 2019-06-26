using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChoMoi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookAuthorsController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public BookAuthorsController(BookStoreContext context)
        {
            _context = context;
        }

        // GET: api/BookAuthors
        [HttpGet]
        public IEnumerable<BookAuthors> GetBookAuthors()
        {
            var res = _context.BookAuthors.AsEnumerable();
            return res;
        }

        // GET: api/BookAuthors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookAuthors([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookAuthors = await _context.BookAuthors.FindAsync(id);

            if (bookAuthors == null)
            {
                return NotFound();
            }

            return Ok(bookAuthors);
        }

        // PUT: api/BookAuthors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookAuthors([FromRoute] long id, [FromBody] BookAuthors bookAuthors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookAuthors.Book.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookAuthors).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookAuthorsExists(id))
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

        // POST: api/BookAuthors
        [HttpPost]
        public async Task<IActionResult> PostBookAuthors([FromBody] BookAuthors bookAuthors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BookAuthors.Add(bookAuthors);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BookAuthorsExists(bookAuthors.Book.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBookAuthors", new { id = bookAuthors.Book.Id }, bookAuthors);
        }

        // DELETE: api/BookAuthors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAuthors([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookAuthors = await _context.BookAuthors.FindAsync(id);
            if (bookAuthors == null)
            {
                return NotFound();
            }

            _context.BookAuthors.Remove(bookAuthors);
            await _context.SaveChangesAsync();

            return Ok(bookAuthors);
        }

        private bool BookAuthorsExists(long id)
        {
            return _context.BookAuthors.Any(e => e.Book.Id == id);
        }
    }
}