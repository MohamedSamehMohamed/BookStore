using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorDbRepository: IBookStoreRepository<Author>
    {
        BookStoreDbContext db; 
        public AuthorDbRepository(BookStoreDbContext db)
        {
            this.db = db;
        }
        public void Add(Author NewAuthor)
        {
            db.Authors.Add(NewAuthor);
            Commit();
        }

        public void Delete(int Id)
        {
            Author author = Find(Id);
            db.Authors.Remove(author);
            Commit();
        }

        public Author Find(int Id)
        {
            Author author = db.Authors.SingleOrDefault(a => a.Id == Id);
            return author;
        }

        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

        public List<Author> Search(string term)
        {
            var result = db.Authors.Where(a => a.FullName.Contains(term)).ToList();
            return result;
        }

        public void Update(int Id, Author NewAuthor)
        {
            db.Authors.Update(NewAuthor);
            Commit();
        }

        public void Update(Author NewAuthor)
        {
            db.Authors.Update(NewAuthor);
            Commit();
        }

        void Commit()
        {
            db.SaveChanges();
        }
    }
}
