using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Week9_3.Models
{
    public class CmsRepository
    {
        private readonly CmsContext context;

        public CmsRepository(CmsContext context)
        {
            this.context = context;
        }

        public List<Page> GetRandomPages(int count)
        {
            return context.Pages.OrderBy(p => Guid.NewGuid()).Take(count).ToList();
        }

        public IQueryable<Page> GetFilteredSortedPages(string urlSearch, string titleSearch, string order)
        {
            var pages = from p in context.Pages
                        select p;

            if (!String.IsNullOrEmpty(urlSearch))
            {
                pages = pages.Where(p => p.UrlName.Contains(urlSearch));
            }

            if (!String.IsNullOrEmpty(titleSearch))
            {
                pages = pages.Where(p => p.Title.Contains(titleSearch));
            }

            if (!String.IsNullOrEmpty(order))
            {
                switch (order)
                {
                    case "UrlAsc": pages = pages.OrderBy(p => p.UrlName); break;
                    case "UrlDesc": pages = pages.OrderByDescending(p => p.UrlName); break;
                    case "TitleAsc": pages = pages.OrderBy(p => p.Title); break;
                    case "TitleDesc": pages = pages.OrderByDescending(p => p.Title); break;
                    case "ContAsc": pages = pages.OrderBy(p => p.Content); break;
                    case "ContDesc": pages = pages.OrderByDescending(p => p.Content); break;
                    case "DateAsc": pages = pages.OrderBy(p => p.AddedDate); break;
                    case "DateDesc": pages = pages.OrderByDescending(p => p.AddedDate); break;
                    case "DescAsc": pages = pages.OrderBy(p => p.Description); break;
                    case "DescDesc": pages = pages.OrderByDescending(p => p.Description); break;
                }
            }

            return pages;
        }

        public void AddPage(Page page)
        {
            context.Pages.Add(page);
            context.SaveChanges();
            context.Entry(page).State = EntityState.Detached;
        }

        public void UpdatePage(Page page)
        {
            context.Pages.Update(page);
            context.SaveChanges();
            context.Entry(page).State = EntityState.Detached;
        }

        public void DeletePageConfirmed(int id)
        {
            var page = context.Pages.SingleOrDefault(m => m.PageId == id);
            context.Pages.Remove(page);
            context.SaveChanges();
        }

        public Page GetPageById(int? id)
        {
            var page = context.Pages.AsNoTracking().SingleOrDefault(m => m.PageId == id);
            return page;
        }

        public bool UrlExists(string UrlName)
        {
            var duplicateExists = context.Pages.AsNoTracking().Any(p => p.UrlName == UrlName);

            return duplicateExists;
        }

        public bool PageExists(int id)
        {
            var res = context.Pages.AsNoTracking().Any(p => p.PageId == id);

            return res;
        }
    }
}
