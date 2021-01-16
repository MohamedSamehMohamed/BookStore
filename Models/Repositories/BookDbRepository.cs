using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Models.Repositories
{
    public class BookDbRepository: IBookStoreRepository<Book>
    {
        BookStoreDbContext db;
        public BookDbRepository(BookStoreDbContext db)
        {
            this.db = db;
        }
        public void Add(Book NewBook)
        {
            db.Books.Add(NewBook);
            Commit();
        }

        public void Delete(int Id)
        {
            Book book = Find(Id);
            db.Books.Remove(book);
            Commit();
        }

        public Book Find(int Id)
        {
            Book book = db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == Id);
            return book;
        }

        public IList<Book> List()
        {
            return db.Books.Include(a => a.Author).ToList();
        }

        public void Update(int Id, Book NewBook)
        {
            db.Books.Update(NewBook);
            Commit();
        }
        public void Update(Book NewBook)
        {
            db.Books.Update(NewBook);
            Commit();
        }
        public List<Book> Search(string term)
        {
            var result = db.Books.Include(a => a.Author).
                Where(b => 
                b.Title.Contains(term)||
                b.Description.Contains(term)||
                b.Author.FullName.Contains(term)
                ).ToList();
            return result;
        }
        void Commit()
        {
            db.SaveChanges();
        }
    }
}
