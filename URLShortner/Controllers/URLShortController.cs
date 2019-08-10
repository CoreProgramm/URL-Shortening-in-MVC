using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using URLShortner.Models;

namespace URLShortner.Controllers
{
    public class URLShortController : Controller
    {
        bitly b = new bitly();
        // GET: URLShort
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(CustomURL Urls)
        {
            Urls.FullUrl = Urls.FullUrl;
            Urls.shortUrl = b.shorten(Urls.FullUrl);
            return View(Urls);
        }
        public ActionResult Test()
        {
            return View();
        }
    }
}