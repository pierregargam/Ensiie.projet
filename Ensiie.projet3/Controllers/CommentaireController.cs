using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;

namespace Ensiie.projet3.Controllers
{
    public class CommentaireController : Controller
    {

        testEntities8 _db = new testEntities8();

        public ActionResult Index(int id)
        {
            var cmt = (from m in _db.Commentaire
                       where m.message_id == id
                       select m);

            return View(cmt.ToList());
        }

        public ActionResult Create(int id)
        {

            HttpContext.Session["id_message"] = id;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "id")] Commentaire_ cmt)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Create", "Commentaire", new { id = HttpContext.Session["id_message"] });

            try
            {
                _db.AddToCommentaire(cmt);
                _db.SaveChanges();

                if (HttpContext.Session["id_theme"] != null)
                    return RedirectToAction("Index", "Commentaire", new { id = HttpContext.Session["id_message"] });

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

                var msg = (from m in _db.Commentaire
                           where m.id == id && m.collaborateur_id == agent_id
                           select m).Count();
                if (msg > 0)
                { verif = true; }

            }

            if (
                (HttpContext.Session["log_admin"] != null && (int)HttpContext.Session["log_admin"] == 1)
                ||
                (verif)
                )
            {

                var cmt = (from m in _db.Commentaire
                           where m.id == id
                           select m).First();

                try
                {
                    // TODO: Add delete logic here

                    _db.DeleteObject(cmt);
                    _db.SaveChanges();

                    if (HttpContext.Session["id_theme"] != null)
                    { return RedirectToAction("Index", "Message", new { id = HttpContext.Session["id_theme"] }); }

                    else return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return View();
                }
            }
            else
            { return RedirectToAction("Index", "Home"); }
        }

    }
}
