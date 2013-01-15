using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;




namespace Ensiie.projet3.Controllers
{
    public class NewsController : Controller
    {
        testEntities8 _db = new testEntities8();

        public ActionResult Index()
        {
            
            var news = (from m in _db.News
                        where (m.dating.Value.Day < DateTime.Today.Day)
                        select m);

            return RedirectToAction("Index", "Home", news);

        }

        //**********************************************************************

        public ActionResult Create()
        {
            if (HttpContext.Session["id"] == null) return RedirectToAction("Index");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "id")] News_ newToCreate)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                _db.AddToNews(newToCreate);

                var agent_id = (int)HttpContext.Session["id"];

                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //**********************************************************************

        public ActionResult Delete(int id)
        {
            if (HttpContext.Session["id"] == null) return RedirectToAction("Index");
            var ag = (from m in _db.News
                      where m.id == id
                      select m).First();

            return View(ag);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(News_ newToDel)
        {
            if (HttpContext.Session["id"] == null) return RedirectToAction("Index");

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("bad1");
                return View();
            }

            var news = (from m in _db.News
                      where m.id == newToDel.id
                      select m).First();

            /*******************/
            /*
            var nb_news = 0;

            while ((nb_news = (from mm in _db.Like_news
                              where mm.news_id == news.id
                              select mm).Count()) > 0)
            {
                var like_n = (from mm in _db.Like_news
                           where mm.news_id == news.id
                           select mm).First();
                try
                {
                    _db.DeleteObject(like_n);
                    _db.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            */
            /*******************/

            try
            {
                _db.DeleteObject(news);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Cette nouvelle n'a pas été supprimée");
                return View(news);
            }
        }


        public ActionResult Like(int id)
        {

            if (HttpContext.Session["id"] == null) return RedirectToAction("Index", "Home");

            Like_news_ ln = new Like_news_();
            ln.collaborateur_id = (int)HttpContext.Session["id"];
            ln.news_id = id;
            _db.AddToLike_news(ln);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

    }
}
