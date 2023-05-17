using Entities.Models;
using Repository.Contracts;
using Repository.EfCore;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookManager : IBookService
    {
        readonly IRepositoryManager _manager;

        public BookManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public Book CreateOneBook(Book book)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            _manager.Book().CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var result = _manager.Book().FindByCondition(i => i.Id.Equals(id), trackChanges).SingleOrDefault();
            if (result != null)
            {
                throw new Exception($"Book with id:{id} could not found");
            }
            _manager.Book().DeleteOneBook(result);
            _manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.Book().GetAllBooks(trackChanges);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            return _manager.Book().GetOneBookById(id,trackChanges);
        }

        public void UpdateOneBook(int id, Book book, bool trackChanges)
        {
            if (book is null)
            {
                throw new ArgumentNullException($"Book with id:{id} could not found");
            }
            var entity = _manager.Book().FindByCondition(i => i.Id.Equals(id), trackChanges).SingleOrDefault();
            if (entity != null)
            {
                throw new Exception($"Book with id:{id} could not found");
            }
            entity.Title = book.Title;
            entity.Price = book.Price;
            _manager.Book().UpdateOneBook(entity);
            _manager.Save();
        }
    }
}
