using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Week9_2.Data;
using Week9_2.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace Week9_2.Controllers
{
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Pages
        public async Task<IActionResult> Index(string titleSearch = null,
            int pageSize = 5, int pageNumber = 0)
        {
            var client = new HttpClient();
            string requestUri = String.Format(@"http://localhost:65013/api/pages?titlesearch={0}", titleSearch == null ? "" : titleSearch);
            var jsonResponse = client.GetStringAsync(requestUri);
            var pages = JsonConvert.DeserializeObject<List<Page>>(await jsonResponse);

            PagesListModel result = new PagesListModel();
            result.PageSize = pageSize;
            result.CurPage = pageNumber;
            result.MaxPage = (pages.Count() + pageSize - 1) / pageSize;
            result.pages = pages.Skip(pageNumber * pageSize).Take(pageSize).ToList();

            return View(result);
        }

        // GET: Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = GetPageById(id);
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
                var client = new HttpClient();
                page.PageId = 888;
                var jsonString = JsonConvert.SerializeObject(page);
                var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                await client.PostAsync("http://localhost:65013/api/pages", content);

                return RedirectToAction("Index");
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

            Page page = await GetPageById(id);
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
                    var client = new HttpClient();
                    var jsonString = JsonConvert.SerializeObject(page);
                    string requestUri = String.Format(@"http://localhost:65013/api/pages/{0}", id.ToString());
                    var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
                    await client.PutAsync(requestUri, content);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.PageId))
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

            Page page = await GetPageById(id);
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
            var client = new HttpClient();
            string requestUri = String.Format(@"http://localhost:65013/api/pages/{0}", id.ToString());
            await client.DeleteAsync(requestUri);
            return RedirectToAction("Index");
        }

        private async Task<Page> GetPageById(int? id)
        {
            var client = new HttpClient();
            string requestUri = String.Format(@"http://localhost:65013/api/pages/{0}", id.ToString());
            var jsonResponse = client.GetStringAsync(requestUri);
            var msg = await jsonResponse;
            var page = JsonConvert.DeserializeObject<Page>(msg);

            return page;
        }

        private bool PageExists(int id)
        {
            return GetPageById(id) != null;
        }

    }
}
