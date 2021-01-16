using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public interface IBookStoreRepository<T>
    {
        IList<T> List();
        void Add(T Entity);
        void Update(int Id, T Entity);
        void Update(T Entity);
        void Delete(int Id);
        T Find(int Id);
        List<T> Search(string term);
    }
}
