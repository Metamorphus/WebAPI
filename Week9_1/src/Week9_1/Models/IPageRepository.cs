using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Week9_1.Models
{
    public interface IPageRepository
    {
        void Add(Page item);
        IEnumerable<Page> GetAll(string titleSearch, int page, int pageSize);
        Page Find(int key);
        void Remove(int key);
        void Update(Page item);
    }
}
