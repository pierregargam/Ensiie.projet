using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;

namespace Ensiie.projet3.Controllers
{
    public class BossController : Controller
    {

        testEntities8 _db = new testEntities8();

        public ActionResult Index()
        {

            if (
                ((HttpContext.Session["log_boss"] == null) || !HttpContext.Session["log_boss"].Equals(1))
                )
            {
                return RedirectToAction("Index", "Home");
            }

            return View(_db.stat_light.ToList());
        }
             
    }
}
