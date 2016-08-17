using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Week9_3.Models;

namespace Week9_3.Controllers
{
    [RequireHttps]
    public class PagesController : Controller
    {
        public CmsRepository repository;

        public PagesController(CmsRepository rep)
        {
            repository = rep;
        }

        // GET: Pages
        public async Task<IActionResult> Index(string urlSearch, string titleSearch,
            string order, int pageNumber = 0)
        {
            var pages = repository.GetFilteredSortedPages(urlSearch, titleSearch, order);

            const int pageSize = 5;
            PagesListModel result = new PagesListModel();
            result.CurPage = pageNumber;
            result.MaxPage = (pages.Count() + pageSize - 1) / pageSize;
            result.pages = await pages.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync();

            return View(result);
        }

        // GET: Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = repository.GetPageById(id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // GET: Pages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PageId,AddedDate,Content,Description,Title,UrlName")] Page page)
        {
            if (ModelState.IsValid)
            {
                if (repository.UrlExists(page.UrlName))
                {
                    ModelState.AddModelError(string.Empty, "This URL already exists");
                    return View(page);
                }
                else
                {
                    repository.AddPage(page);
                    return RedirectToAction("Index");
                }
            }
            return View(page);
        }

        // GET: Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = repository.GetPageById(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageId,AddedDate,Content,Description,Title,UrlName")] Page page)
        {
            if (id != page.PageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    repository.UpdatePage(page);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!repository.PageExists(page.PageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET: Pages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = repository.GetPageById(id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            repository.DeletePageConfirmed(id);
            return RedirectToAction("Index");
        }

        /*[HttpPost]
        public JsonResult urlExists(string UrlName)
        {
            var duplicateExists = _context.Pages.Any(p => p.UrlName == UrlName);

            return Json(duplicateExists == false);
        }*/
    }
}
