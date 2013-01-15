using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;
namespace Ensiie.projet3.Controllers
{
    public class AbController : Controller
    {

        testEntities8 _db = new testEntities8();

        public ActionResult Index()
        {

            if (
                ((HttpContext.Session["log_admin"] == null) || !HttpContext.Session["log_admin"].Equals(1))
                &&
                ((HttpContext.Session["log_collabo"] == null) || !HttpContext.Session["log_collabo"].Equals(1))
                &&
                ((HttpContext.Session["log_boss"] == null) || !HttpContext.Session["log_boss"].Equals(1))
                )
            {
                return RedirectToAction("Index", "Home");
            }
            //Selon admin/collabo: différentes redirection!!
            return View(_db.Theme.ToList());
        }

        public ActionResult Abonner_t(int id)
        {

            if (HttpContext.Session["id"] == null)
            {
                ModelState.AddModelError("", "Vous n'avez plus d'identifiant!");
                return RedirectToAction("Index", "Home");
            }

            access a = new access();
            int id_agent = (int) HttpContext.Session["id"];
            System.Diagnostics.Debug.WriteLine("identifiant en mousse abonnement: " + HttpContext.Session["id"]);

            bool verif1 = true;
            var ret = (from m in _db.Abonnement
                       where m.collaborateur_id == id_agent && m.theme_id == id
                       select m).Count();
            if (ret > 0) { verif1 = false; }

            if (verif1)
            {
                bool verif = a.insert_ab(id, id_agent);
                HttpContext.Session["id"] = id_agent;

                if (verif == false)
                    ModelState.AddModelError("", "Problème d'insertion.");
            }
            return View("Index", _db.Theme.ToList());    
        } 
 
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session["id"] == null)
            {
                ModelState.AddModelError("", "Vous n'avez plus d'identifiant!");
                return RedirectToAction("Index", "Home");
            }

            access a = new access();
            int id_agent = (int)HttpContext.Session["id"];
            System.Diagnostics.Debug.WriteLine("identifiant en mousse delete: " + HttpContext.Session["id"]);
            bool verif = a.delete_ab(id, id_agent);
            HttpContext.Session["id"] = id_agent;

            if (verif == false)
                ModelState.AddModelError("", "Problème de suppression.");

            return View("Index", _db.Theme.ToList()); 
        }

    }
}
