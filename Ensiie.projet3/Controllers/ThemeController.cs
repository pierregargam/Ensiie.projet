using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;

namespace Ensiie.projet3.Controllers
{
    public class ThemeController : Controller
    {
        testEntities8 _db = new testEntities8();

        public ActionResult Index()
        {
            if ((HttpContext.Session["log_admin"] == null) || !HttpContext.Session["log_admin"].Equals(1))
            //if (!HttpContext.Session["log_admin"].Equals(1))
            {
                return RedirectToAction("Index", "Home");
            }


            var themes = from theme in _db.Theme
                         select theme;

            var abonn = from abon in _db.Abonnement
                        join theme in _db.Theme on abon.theme_id equals theme.id
                        join agent in _db.Agent on abon.collaborateur_id equals agent.id
                        select abon;

            index_theme i_t = new index_theme(themes,abonn);

            //return View(_db.Theme.ToList());
            return View(i_t);
        }

        //**********************************************************************

        public ActionResult Create()
        {
            /*
            if ((HttpContext.Session["log_admin"] == null) || !HttpContext.Session["log_admin"].Equals(1))
            //if (!HttpContext.Session["log_admin"].Equals(1))
            {
                return RedirectToAction("Index", "Home");
            }
            */
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "id")] Theme_ themeToCreate)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                Stat_ s = new Stat_();
                s.theme_id = themeToCreate.id;
                s.nombre_vues = 0;
                _db.AddToStat(s);

                _db.AddToTheme(themeToCreate);
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
            var ag = (from m in _db.Theme
                      where m.id == id
                      select m).First();

            return View(ag);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Theme_ themeToDel)
        {
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("bad1");
                return View();
            }
            try
            {
                access da = new access();
                da.delete_tables_ref_theme3(themeToDel.id);
                
                return RedirectToAction("Index");
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("bad2");
                return View();
            }
        }

        public ActionResult Delete_user_abon(int id_theme, int id_agent)
        {
            var ab = (from m in _db.Abonnement
                      where m.theme_id == id_theme && m.collaborateur_id == id_agent
                      select m).First();

            try
            {
                _db.DeleteObject(ab);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Cet abonné n'a pas été supprimé");
                return View();
            }

            return View();
        }

    }
}
