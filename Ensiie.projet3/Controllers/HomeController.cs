using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;

namespace Ensiie.projet3.Controllers
{
    public class HomeController : Controller
    {


        testEntities8 _db = new testEntities8();

        public ActionResult Index()
        {

            var news = (from m in _db.News
                        where ((m.dating.Value.Day +3) > DateTime.Today.Day )
                       select m);

            int id_agent = 0;
            if ((HttpContext.Session["id"] != null) && ((int)HttpContext.Session["id"] != 0))
            {
                id_agent = (int)HttpContext.Session["id"];
            }

            var theme_ok = from theme in _db.Theme
                        join abon in _db.Abonnement on theme.id equals abon.theme_id
                        join agent in _db.Agent on abon.collaborateur_id equals agent.id
                        where agent.id == id_agent
                        select theme;

            var like_n = from like in _db.Like_news
                       join agent in _db.Agent on like.collaborateur_id equals agent.id
                       where agent.id == id_agent
                       select like;

            var like_all = from like in _db.Like_news
                           select like;

            SomeViewModel svm = new SomeViewModel(news,theme_ok,like_n,like_all);

            return View(svm);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
