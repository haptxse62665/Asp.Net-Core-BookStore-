using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoAPI.ViewModels;
using DemoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using ChoMoi.DTOs;

namespace ChoMoi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookCategoriesController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public BookCategoriesController(BookStoreContext context)
        {
            _context = context;
        }

        //Get all book by category
        /// <summary>
        /// Get all Book by Category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [Route("getAllBookByCategory")]
        [HttpGet]
        public List<BookViewModel> getAllBookByCategory(int categoryId)
        {
            List<BookViewModel> bookViewModels = new List<BookViewModel>();
            var bookList = _context.Book.Where(p => p.Category.Id == categoryId).ToList();
            foreach (var item in bookList)
            {                
                BookViewModel bookViewModel = new BookViewModel();
                bookViewModel.Authors = new List<String>();
                bookViewModel.Title = item.Title;
                bookViewModel.Catergory = item.Category.Name;
                bookViewModel.BuyFromOffline = item.BookBuyOffile.BuyFrom;
                bookViewModel.BuyFromOnline = item.BookBuyOnline.BuyFrom;
                var bookAuthors= item.BookAuthors.Where(x=> x.Book.Id == item.Id).Select(x => x.User).ToList();
                foreach (var user in bookAuthors)
                {
                    if(user!=null)
                    bookViewModel.Authors.Add(user.UserName);
                }
                bookViewModel.Publisher = item.Publisher.Name;
                bookViewModels.Add(bookViewModel);
            }
            return bookViewModels;
        }

        // GET: api/BookCategories
        /// <summary>
        /// Test Role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public IEnumerable<BookCategory> GetBookCategory()
        {
            return _context.BookCategory;
        }

        // GET: api/BookCategories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookCategory([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookCategory = await _context.BookCategory.FindAsync(id);

            if (bookCategory == null)
            {
                return NotFound();
            }

            return Ok(bookCategory);
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookCategory"></param>
        /// <returns></returns>
        // PUT: api/BookCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookCategory([FromRoute] long id, [FromBody] BookCategory bookCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookCategory).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookCategoryExists(id))
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
        /// Add Book Category
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        // POST: api/BookCategories
        [HttpPost]
        public async Task<IActionResult> PostBookCategory(string categoryName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            BookCategory bookCategory = new BookCategory();
            bookCategory.CreatedDate = DateTime.UtcNow;
            bookCategory.Name = categoryName;
            //string token = Request.Headers["Authorization"];
            _context.BookCategory.Add(bookCategory);
            _context.SaveChanges();

            return CreatedAtAction("GetBookCategory", new { id = bookCategory.Id }, bookCategory);
        }

        // DELETE: api/BookCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookCategory([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookCategory = await _context.BookCategory.FindAsync(id);
            if (bookCategory == null)
            {
                return NotFound();
            }

            _context.BookCategory.Remove(bookCategory);
            await _context.SaveChangesAsync();

            return Ok(bookCategory);
        }

        private bool BookCategoryExists(long id)
        {
            return _context.BookCategory.Any(e => e.Id == id);
        }
    }
}