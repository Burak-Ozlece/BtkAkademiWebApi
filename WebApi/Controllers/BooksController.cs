using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        readonly RepositoryContext _context;

        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook(int id)
        {
            try
            {
                var book = _context.Books.Where(x => x.Id.Equals(id)).SingleOrDefault();

                if (book is null)
                    return NotFound();
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                    return BadRequest();

                _context.Books.Add(book);
                _context.SaveChanges();
                return StatusCode(201, book);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name ="id")] int id, [FromBody] Book book)
        {
            try
            {
                if (book is null || id == 0)
                    return BadRequest();

                var entity = _context.Books.Where(i => i.Id == id).SingleOrDefault();

                if (entity is null)
                    return NotFound();

                if (id != book.Id)
                    return BadRequest();

                entity.Title = book.Title;
                entity.Price = book.Price;

                _context.SaveChanges();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var entity = _context.Books.Where(i=> i.Id.Equals(id)).SingleOrDefault();

                if (entity is null)
                    return NotFound();

                _context.Books.Remove(entity);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                // check entity
                var entity = _context
                    .Books
                    .Where(b => b.Id.Equals(id))
                    .SingleOrDefault();

                if (entity is null)
                    return NotFound(); // 404

                bookPatch.ApplyTo(entity);
                _context.SaveChanges();

                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}