using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Week9_1.Models
{
    public class PageRepository : IPageRepository
    {
        private readonly CmsContext context;

        public PageRepository(CmsContext context)
        {
            this.context = context;
        }

        public IEnumerable<Page> GetAll(string titleSearch, int page, int pageSize)
        {
            var pages = from p in context.Pages
                        select p;

            if (!String.IsNullOrEmpty(titleSearch))
            {
                pages = pages.Where(p => p.Title.Contains(titleSearch));
            }

            return pages.Skip(page * pageSize).Take(pageSize).ToList();
        }

        public void Add(Page page)
        {
            context.Pages.Add(page);
            context.SaveChanges();
            context.Entry(page).State = EntityState.Detached;
        }

        public Page Find(int id)
        {
            var page = context.Pages.AsNoTracking().SingleOrDefault(m => m.PageId == id);
            return page;
        }

        public void Remove(int id)
        {
            var page = context.Pages.AsNoTracking().SingleOrDefault(m => m.PageId == id);
            context.Pages.Remove(page);
            context.SaveChanges();
        }

        public void Update(Page page)
        {
            context.Pages.Update(page);
            context.SaveChanges();
            context.Entry(page).State = EntityState.Detached;
        }
    }
}
