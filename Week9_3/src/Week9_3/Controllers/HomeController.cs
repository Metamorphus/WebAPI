using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Week9_3.Models;

namespace Week9_3.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public CmsRepository repository;

        public HomeController(CmsRepository rep)
        {
            repository = rep;
        }

        public IActionResult Index()
        {
            var randomPages = repository.GetRandomPages(3);
            return View(randomPages);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
