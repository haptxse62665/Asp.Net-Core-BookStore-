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
    public class PublishersController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public PublishersController(BookStoreContext context)
        {
            _context = context;
        }

        // GET: api/Publishers
        [HttpGet]
        public IEnumerable<Publisher> GetPublisher()
        {
            return _context.Publisher;
        }

        // GET: api/Publishers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisher([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = await _context.Publisher.FindAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }

        // PUT: api/Publishers/5
        /// <summary>
        /// Update Publisher
        /// </summary>
        /// <param name="id"></param>
        /// <param name="publisher"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult PutPublisher([FromRoute] long id, [FromBody] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != publisher.Id)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
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

        /// <summary>
        /// Add Publisher
        /// </summary>
        /// <param name="publisher"></param>
        /// <returns></returns>
        // POST: api/Publishers
        [HttpPost]
        public IActionResult PostPublisher(string publisherName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Publisher publisher = new Publisher();
            publisher.CreatedDate = DateTime.UtcNow;               
            publisher.Name = publisherName;
            _context.Publisher.Add(publisher);
            _context.SaveChanges();

            return CreatedAtAction("GetPublisher", new { id = publisher.Id }, publisher);
        }

        // DELETE: api/Publishers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var publisher = await _context.Publisher.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _context.Publisher.Remove(publisher);
            await _context.SaveChangesAsync();

            return Ok(publisher);
        }

        private bool PublisherExists(long id)
        {
            return _context.Publisher.Any(e => e.Id == id);
        }
    }
}