using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ChoMoi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorContactsController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public AuthorContactsController(BookStoreContext context)
        {
            _context = context;
        }


        //Get contact by AuthorID
        /// <summary>
        /// Get Contact by Author ID
        /// </summary>
        /// <param name="authorID"></param>
        /// <returns></returns>
        [Route("getContactByAuthorID")]
        [HttpGet]
        public List<AuthorContactViewModel> GetContactByAuthorID(String authorID)
        {
            List<AuthorContactViewModel> authorContactViewModels = new List<AuthorContactViewModel>();
            var contactList = _context.AuthorContact
                                      .Where(p => p.User.Id == authorID && p.Status == true).ToList();
           foreach(var item in contactList)
            {
                authorContactViewModels.Add(
                    new AuthorContactViewModel() { Address = item.Address, ContactNumber = item.ContactNumber });
            }
            return authorContactViewModels;
        }

        // GET: api/AuthorContacts
        [HttpGet]
        public IEnumerable<AuthorContact> GetAuthorContact()
        {
            return _context.AuthorContact;
        }

        // GET: api/AuthorContacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorContact([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorContact = await _context.AuthorContact.FindAsync(id);

            if (authorContact == null)
            {
                return NotFound();
            }

            return Ok(authorContact);
        }

        // PUT: api/AuthorContacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthorContact([FromRoute] long id, [FromBody] AuthorContact authorContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != authorContact.Id)
            {
                return BadRequest();
            }

            _context.Entry(authorContact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorContactExists(id))
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

        // POST: api/AuthorContacts
        [HttpPost]
        public async Task<IActionResult> PostAuthorContact([FromBody] AuthorContact authorContact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AuthorContact.Add(authorContact);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AuthorContactExists(authorContact.Id))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAuthorContact", new { id = authorContact.Id }, authorContact);
        }

        // DELETE: api/AuthorContacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthorContact([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorContact = await _context.AuthorContact.FindAsync(id);
            if (authorContact == null)
            {
                return NotFound();
            }

            _context.AuthorContact.Remove(authorContact);
            await _context.SaveChangesAsync();

            return Ok(authorContact);
        }

        private bool AuthorContactExists(long id)
        {
            return _context.AuthorContact.Any(e => e.Id == id);
        }
    }
}