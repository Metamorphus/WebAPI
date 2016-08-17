using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Week9_1.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Week9_1.Controllers
{
    [Route("api/pages")]
    public class PagesController : Controller
    {
        IPageRepository repository;

        public PagesController(IPageRepository rep)
        {
            repository = rep;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Page> Get(string titleSearch = null, int page = 0, int pageSize = 1000)
        {
            return repository.GetAll(titleSearch, page, pageSize);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var page = repository.Find(id);
            if (page == null)
            {
                return NotFound();
            }
            return new ObjectResult(page);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            repository.Add(page);
            return CreatedAtRoute("GetPage", new { id = page.PageId }, page);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Page page)
        {
            if (page == null)
            {
                return BadRequest();
            }

            var oldPage = repository.Find(id);
            if (oldPage == null)
            {
                return NotFound();
            }
            page.PageId = id;

            repository.Update(page);
            return new NoContentResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var page = repository.Find(id);
            if (page == null)
            {
                return NotFound();
            }

            repository.Remove(id);
            return new NoContentResult();
        }
    }
}
