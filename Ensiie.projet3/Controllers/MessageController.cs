using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;

namespace Ensiie.projet3.Controllers
{
    public class MessageController : Controller
    {

        testEntities8 _db = new testEntities8();

        public ActionResult Index(int id)
        {
            var msg = (from m in _db.Message
                      where m.theme_id == id
                      select m);

            /****************/

            access da = new access();
            da.maj_stat(id);

            /****************/

            HttpContext.Session["id_theme"] = id;

            return View(msg.ToList());
        }



        public ActionResult Create()
        {
            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "id")] Message_ msg)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Create", "Message");

            try
            {
                _db.AddToMessage(msg);
                _db.SaveChanges();

                /*ici, d abord creer le theme dans stat, puis faire un update*/

                if (HttpContext.Session["id_theme"] != null)
                    return RedirectToAction("Index", "Message", new { id = HttpContext.Session["id_theme"] });

                else
                    return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index","Home");
            }
        }
        
        //
        // GET: /Message/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Message/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {

            bool verif = false;

            if(HttpContext.Session["id"] != null)
            {
                int agent_id = (int)HttpContext.Session["id"];

                var msg = (from m in _db.Message
                   where m.id == id && m.collaborateur_id == agent_id
                   select m).Count();
                if(msg>0)
                {verif = true;}

            }

            if (
                (HttpContext.Session["log_admin"] != null && (int)HttpContext.Session["log_admin"] == 1)
                ||
                (verif)
                )
            {

                var msg = (from m in _db.Message
                           where m.id == id
                           select m).First();

                try
                {
                    _db.DeleteObject(msg);
                    _db.SaveChanges();

                    if (HttpContext.Session["id_theme"] != null)
                    { return RedirectToAction("Index", "Message", new { id = HttpContext.Session["id_theme"] }); }

                    else return RedirectToAction("Index", "Home");
                }
                catch
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            { return RedirectToAction("Index", "Home"); }
        }

    }
}
