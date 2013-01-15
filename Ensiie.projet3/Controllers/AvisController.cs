using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;

namespace Ensiie.projet3.Controllers
{
    public class AvisController : Controller
    {

        testEntities8 _db = new testEntities8();

        public ActionResult Index(int id)
        {
            var avis = (from m in _db.Avis_news
                       where m.news_id == id
                       select m);

            return View(avis.ToList());
        }

        public ActionResult Create(int id)
        {

            HttpContext.Session["id_news"] = id;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "id")] Avis_news_ avis)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Create", "Avis", new { id = HttpContext.Session["id_news"] });

            try
            {
                //_db.AddToAvis_news(avis);
                _db.AddObject("Avis_news", avis);
                _db.SaveChanges();

                if (HttpContext.Session["id_news"] != null)
                    return RedirectToAction("Index", "Avis", new { id = HttpContext.Session["id_news"] });

                else
                    return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }




        /*
        public ActionResult Delete(int id)
        {
            return View();
        }
        */
        
        public ActionResult Delete(int id)
        {

            bool verif = false;

            if (HttpContext.Session["id"] != null)
            {
                int agent_id = (int)HttpContext.Session["id"];

                var avis = (from m in _db.Avis_news
                           where m.id == id && m.collaborateur_id == agent_id
                           select m).Count();
                if (avis > 0)
                { verif = true; }

            }

            if (
                (HttpContext.Session["log_admin"] != null && (int)HttpContext.Session["log_admin"] == 1)
                ||
                (verif)
                )
            {

                var avis = (from m in _db.Avis_news
                           where m.id == id
                           select m).First();

                try
                {
                    // TODO: Add delete logic here

                    _db.DeleteObject(avis);
                    _db.SaveChanges();

                    if (HttpContext.Session["id_news"] != null)
                    { return RedirectToAction("Index", "News", new { id = HttpContext.Session["id_news"] }); }

                    else return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return View();
                }
            }
            else
            { return RedirectToAction("Index", "News", new { id = HttpContext.Session["id_news"] }); }
        }
        
    }
}
