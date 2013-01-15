using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ensiie.projet3.Models;
using System.Web.Security;

namespace Ensiie.projet3.Controllers
{
    public class Log_RegisterController : Controller
    {
        testEntities8 _db = new testEntities8();

        public ActionResult fetch_admin()
        {

            var agents = (from m in _db.Agent
                                where m.type == 1
                                select m).First();

            return View(_db.Agent.ToList());
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(Agent_ model, string returnUrl)
        {
            if (ModelState.IsValid)
            {


                if (Membership.ValidateUser(model.name, model.password))
                {
                    /***********************/
                    /***********************/
                    int admin = (from m in _db.Agent
                                 where m.type == 1 && m.name == model.name && m.password == model.password
                                 select m).Count();

                    if (admin > 0)
                    {
                        HttpContext.Session["log_admin"] = 1;
                    }

                    int collabo = (from m in _db.Agent
                                   where m.type == 2 && m.name == model.name && m.password == model.password
                                   select m).Count();

                    if (collabo > 0)
                    {
                        HttpContext.Session["log_collabo"] = 1;
                    }

                    int boss = (from m in _db.Agent
                                   where m.type == 4 && m.name == model.name && m.password == model.password
                                   select m).Count();

                    if (boss > 0)
                    {
                        HttpContext.Session["log_boss"] = 1;
                    }

                    var nb_a = 0;
                    nb_a = (from m in _db.Agent
                            where m.name == model.name && m.password == model.password
                            select m.id).Count();

                    if (nb_a > 0)
                    {
                        var id = (from m in _db.Agent
                                  where m.name == model.name && m.password == model.password
                                  select m.id).First();

                        HttpContext.Session["id"] = id;
                    }

                    /***********************/
                    /***********************/

                    FormsAuthentication.SetAuthCookie(model.name, true);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session["log_admin"] = 0;
            HttpContext.Session["log_collabo"] = 0;
            HttpContext.Session["log_boss"] = 0;
            HttpContext.Session["id"] = 0;
            return RedirectToAction("Index", "Log_Register");
        }

        public ActionResult Index()
        {
            /*
            Membership.DeleteUser("cahuzac");
            Membership.DeleteUser("titi");
            Membership.DeleteUser("lolo");
            Membership.DeleteUser("toto");
            Membership.DeleteUser("riri");
            Membership.DeleteUser("admin");
            Membership.DeleteUser("loulou");
            */

            if (!Membership.ValidateUser("admin", "123456"))
            {
                Agent_ agentToCreate = new Agent_();
                agentToCreate.dating = DateTime.Now;
                agentToCreate.name = "admin";
                agentToCreate.password = "123456";
                agentToCreate.type = 1;
                agentToCreate.first_name = "admin";
                _db.AddToAgent(agentToCreate);
                _db.SaveChanges();
                MembershipCreateStatus createStatus;
                Membership.CreateUser("admin", "123456", null, null, null, true, null, out createStatus);
            }

            if ((HttpContext.Session["log_admin"] == null) || !HttpContext.Session["log_admin"].Equals(1))
            //if (!HttpContext.Session["log_admin"].Equals(1))
            {
                return RedirectToAction("Index","Home");
            }

            return View(_db.Agent.ToList());
        }

        public ActionResult Details(int id)
        {

            var ag = (from m in _db.Agent
                      where m.id == id
                      select m).First();

            return View(ag);
        }

        public ActionResult Create()
        {
            return View();
        }

        //**********************************************************************

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Agent_ agentToCreate)
        {

            if (!ModelState.IsValid)
                return View();

            if (!Membership.ValidateUser(agentToCreate.name, agentToCreate.password))
            {
                MembershipCreateStatus createStatuss;
                Membership.CreateUser(agentToCreate.name, agentToCreate.password, null, null, null, true, null, out createStatuss);

                if (createStatuss == MembershipCreateStatus.Success)
                {
                    _db.AddToAgent(agentToCreate);
                    _db.SaveChanges();

                    return RedirectToAction("Index", "Log_Register");
                }
                else
                {
                    ModelState.AddModelError("", "Problème d'insertion en base.");
                }

            }
            else
            {
                ModelState.AddModelError("", "Nom déjà existant en base.");
            }

            return View();

            /*
            try
            {
                
                _db.AddToAgent(agentToCreate);
                _db.SaveChanges();
                

                if (!Membership.ValidateUser(agentToCreate.name, agentToCreate.password))
                {
                    MembershipCreateStatus createStatus;
                    Membership.CreateUser(agentToCreate.name, agentToCreate.password, "none@none.com", null, null, true, null, out createStatus);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        System.Diagnostics.Debug.WriteLine("CREATION OK");
                        return RedirectToAction("Index", "Log_Register");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("CREATION FAIL");
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
            */
        }

        //**********************************************************************

        public ActionResult Edit(int id)
        {
            var ag = (from m in _db.Agent
                      where m.id == id
                      select m).First();

            return View(ag);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(Agent_ agentToEdit)
        {
            if (!ModelState.IsValid)
                return View();

            if ((int)HttpContext.Session["id"] == agentToEdit.id)
            {
                ModelState.AddModelError("", "Vous ne pouvez pas vous éditer. Passer par un autre admin (cela évite les problème dûs aux suppressions, sachant que vos variables de session administrateur existent toujours.");
                return View(agentToEdit);
            }

            try
            {

                var originalAgent = (from m in _db.Agent
                                     where m.id == agentToEdit.id
                                     select m).First();

                if (originalAgent.password == "123456" && originalAgent.name == "admin")
                {
                    ModelState.AddModelError("", "Vous ne pouvez pas l'éditer.");
                    return View(agentToEdit);
                }

                Membership.DeleteUser(originalAgent.name);

                MembershipCreateStatus createStatuss;
                Membership.CreateUser(agentToEdit.name, agentToEdit.password, null, null, null, true, null, out createStatuss);

                if (createStatuss == MembershipCreateStatus.Success)
                {

                    _db.ApplyCurrentValues(originalAgent.EntityKey.EntitySetName, agentToEdit);
                    _db.SaveChanges();

                }
                else
                {
                    /*On recrée l'agent qu'on vient de supprimer si on a essayé d'inscrire un agent djà existant*/
                    Membership.CreateUser(originalAgent.name, originalAgent.password, null, null, null, true, null, out createStatuss);
                    ModelState.AddModelError("", "Problème d'insertion en base. Cela est dû à une redondance des termes (nom et password) utilisés (un autre agent similaire existe déjà en base).");
                    return View(agentToEdit);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            var ag = (from m in _db.Agent
                      where m.id == id
                      select m).First();

            return View(ag);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(Agent_ agentToDel)
        {
            if (!ModelState.IsValid)
                return View();

            if ((int)HttpContext.Session["id"] == agentToDel.id)
            {
                ModelState.AddModelError("", "Vous ne pouvez pas vous supprimer. Ce serait tellement dommage..!");
                return View(agentToDel);
            }

            if (agentToDel.password == "123456" && agentToDel.name == "admin")
            {
                ModelState.AddModelError("", "Vous ne pouvez pas le supprimer.");
                return View(agentToDel);
            }

            var ag = (from m in _db.Agent
                      where m.id == agentToDel.id
                      select m).First();

            try
            {

                Membership.DeleteUser(ag.name);

                _db.DeleteObject(ag);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Cet agent est lié à un abonnement, une nouvelle..."
                    +"Vous devez d'abord supprimer ces liens avant de le supprimer lui.");
                return View(ag);
            }
        }
    }
}