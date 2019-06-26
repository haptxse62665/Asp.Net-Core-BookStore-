using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoMoi.DTOs;
using ChoMoi.Helper;
using DemoAPI.Models;
using DemoAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChoMoi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreContext _context;

        public BooksController(BookStoreContext context)
        {
            _context = context;
        }

        //Get all book by User
        /// <summary>
        /// Get All Book by condition
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="fileterKey"></param>
        /// <param name="sortBy"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        [Route("getAllBookByCondition")]
        [HttpGet]
        public PaginationViewModel<BookViewModel> GetAllBookByCondition(string fileterKey="", string sortBy = null, string searchKey = null, int page = 1, int pageSize = 10)
        {
            List<BookViewModel> bookViewModels = new List<BookViewModel>();
            //var bookList = _context.BookAuthors.Where(p => p.User.Id == authorID).ToList();
            var bookList = _context.Book.ToList();
            foreach (var item in bookList)
            {
                BookViewModel bookViewModel = new BookViewModel();
                bookViewModel.Authors = new List<String>();

                bookViewModel.Catergory = item.Category.Name;
                bookViewModel.Publisher = item.Publisher.Name;

                bookViewModel.BuyFromOffline = item.BookBuyOffile.BuyFrom;
                bookViewModel.BuyFromOnline = item.BookBuyOnline.BuyFrom;

                bookViewModel.Title = item.Title;
                bookViewModel.Authors= item.BookAuthors.Where(x => x.Book.Id == item.Id).Select(x => x.User.UserName).ToList(); ;
                bookViewModels.Add(bookViewModel);
            }

            //get all
            if (page == -1)
            {
                PaginationViewModel<BookViewModel> notPagi = new PaginationViewModel<BookViewModel>();
                notPagi.Amount = bookViewModels.Count();
                notPagi.Data = bookViewModels;
                notPagi.TotalCount = bookViewModels.Count();
                notPagi.TotalPage = 1;
                return notPagi;
            }
            //=============


            var entries = bookViewModels;

            //sort
            if(!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals(ObjectFields.BOOK_NAME_INC))
                    entries = entries.OrderBy(o => o.Title).ToList();

                if (sortBy.Equals(ObjectFields.BOOK_NAME_DESC))
                    entries = entries.OrderByDescending(o => o.Title).ToList();

            }

            

            //search
            if (!string.IsNullOrEmpty(searchKey))
            {
                entries = entries.Where(s => s.Title.ToUpper().Contains(searchKey.ToUpper()) || s.Authors.Contains(searchKey)
                                       || s.Catergory.ToUpper().Contains(searchKey) || s.Publisher.ToUpper().Contains(searchKey.ToUpper())).ToList();
            }

            //fileter   
            if (!string.IsNullOrEmpty(fileterKey))
            {
                entries = entries.Where(s => s.Catergory.Equals(fileterKey) || s.Publisher.Equals(fileterKey)
                                        || s.Authors.Equals(fileterKey)).ToList();
            }

            var count = entries.Count(); 

            entries = entries.Skip((page - 1) * pageSize).Take(pageSize).ToList();


            PaginationViewModel<BookViewModel> pagi = new PaginationViewModel<BookViewModel>();
            pagi.Amount = pageSize;
            pagi.Data = entries;
            pagi.TotalCount = count;
            pagi.TotalPage = count%pageSize == 0 ? count / pageSize : count/pageSize + 1;

            return pagi;

        }

        /// <summary>
        /// Get all Book
        /// </summary>
        /// <returns></returns>
        // GET: api/Books
        [HttpGet]
        public List<BookViewModel> GetBook()
        {
            List<Book> books = _context.Book.ToList<Book>();
            List<BookViewModel> booksViewModel =new List<BookViewModel>();
            foreach (var book in books)
            {
                BookViewModel tmp = new BookViewModel();
                tmp.Authors = new List<String>();
                tmp.Title = book.Title;
                tmp.Catergory = book.Category.Name;
                tmp.Publisher = book.Publisher.Name;
                tmp.BuyFromOffline = book.BookBuyOffile.BuyFrom;
                tmp.BuyFromOnline = book.BookBuyOnline.BuyFrom;
                tmp.Authors = book.BookAuthors.Where(x => x.Book.Id == book.Id).Select(x => x.User.UserName).ToList();
                booksViewModel.Add(tmp);
            }
            return booksViewModel;
        }
        /// <summary>
        /// Add new book
        /// </summary>
        /// <param name="insertBookViewModel"></param>
        /// <returns></returns>
        [Route("InsertBook")]
        [HttpPost]
        public ActionResult InsertBook(InsertBookViewModel insertBookViewModel)
        {
            if (insertBookViewModel.Title == null || insertBookViewModel.CategoryId <1 || insertBookViewModel.AuthorIds == null) return BadRequest("Add more information");
            if (insertBookViewModel.BookBuyOnlineId == null && insertBookViewModel.BookBuyOffileId == null) return BadRequest("Insert book buy Id");

            List<string> authors = insertBookViewModel.AuthorIds;

            Book book = new Book();

            book.BookBuyOffileId = insertBookViewModel.BookBuyOffileId;
            book.BookBuyOnlineId = insertBookViewModel.BookBuyOnlineId;

            book.CategoryId = insertBookViewModel.CategoryId;
            book.PublisherId = insertBookViewModel.PublisherId;

            book.Deleted = false;

            book.Title = insertBookViewModel.Title;

            _context.Book.Add(book);

            foreach(string authorId in authors)
            {
                BookAuthors bookAuthors = new BookAuthors();

                bookAuthors.BookId = book.Id;
                bookAuthors.UserId = authorId;
                bookAuthors.CreatedDate = new DateTime();
                try
                {

                    _context.BookAuthors.Add(bookAuthors);
                }
                catch (Exception)
                {

                    return BadRequest("Check your input");
                }

            }

            _context.SaveChanges();
            return Ok();           
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook([FromRoute] long id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        [HttpPost]
        public async Task<IActionResult> PostBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Book.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(book);
        }

        private bool BookExists(long id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}