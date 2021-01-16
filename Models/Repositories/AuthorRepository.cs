using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorRepository : IBookStoreRepository<Author>
    {
        List<Author> Authors; 
        public AuthorRepository()
        {
            Authors = new List<Author>()
            {
                new Author{Id = 1, FullName = "Mohamed Sameh"},
                new Author{Id = 2, FullName = "Mohamed Mostafa"},
                new Author{Id = 3, FullName = "Sami Mohamed"},
            };
        }
        public void Add(Author NewAuthor)
        {
            NewAuthor.Id = Authors.Max(a => a.Id) + 1; 
            Authors.Add(NewAuthor);
        }

        public void Delete(int Id)
        {
            Author author = Find(Id);
            Authors.Remove(author); 
        }

        public Author Find(int Id)
        {
            Author author = Authors.SingleOrDefault(a => a.Id == Id);
            return author; 
        }

        public IList<Author> List()
        {
            return Authors; 
        }

        public List<Author> Search(string term)
        {
            var result = Authors.Where(a => a.FullName.Contains(term)).ToList();
            return result;
        }

        public void Update(int Id, Author NewAuthor)
        {
            Author author = Find(Id);
            author.FullName = NewAuthor.FullName; 
        }

        public void Update(Author Entity)
        {
            throw new NotImplementedException();
        }
    }
}
