/*!
* DisplayMonkey source file
* http://displaymonkey.org
*
* Copyright (c) 2015 Fuel9 LLC and contributors
*
* Released under the MIT license:
* http://opensource.org/licenses/MIT
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisplayMonkey.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace DisplayMonkey.Controllers
{
    public class MemoController : BaseController
    {
        private DisplayMonkeyEntities db = new DisplayMonkeyEntities();

        //
        // GET: /Memo/Details/5

        public ActionResult Details(int id = 0)
        {
            Memo memo = db.Frames.Find(id) as Memo;
            if (memo == null)
            {
                return View("Missing", new MissingItem(id));
            }
            return View(memo);
        }

        //
        // GET: /Memo/Create

        public ActionResult Create()
        {
            Frame frame = TempData[FrameController.SelectorFrameKey] as Frame;
            
            if (frame == null || frame.PanelId == 0)
            {
                return RedirectToAction("Create", "Frame");
            }

            Memo memo = new Memo(frame, db);


            this.FillTemplatesSelectList(db, FrameTypes.Memo);
            
            return View(memo);
        }

        //
        // POST: /Memo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Memo memo)
        {
            if (ModelState.IsValid)
            {
                //memo.Body.Replace("\n", "<br/>");
                db.Frames.Add(memo);
                db.SaveChanges();

                // Om anv�ndaren �r i rollen "butik" ska vi skapa en post i FrameLocation
                if (User.IsInRole("Butik"))
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    string butik = user.PhoneNumber;

                    int userlocation = (from ul in db.Locations
                                        where ul.Name.Contains(user.PhoneNumber)
                                        select ul.LocationId).FirstOrDefault();

                    int lastcreatedframe = (from lf in db.Frames
                                            orderby lf.FrameId descending
                                            select lf.FrameId).FirstOrDefault();


                    Frame frame = db.Frames.Find(lastcreatedframe);
                    Location location = db.Locations.Find(userlocation);
                    frame.Locations.Add(location);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Frame");
            }


            this.FillTemplatesSelectList(db, FrameTypes.Memo, memo.TemplateId);

            return View(memo);
        }

        //
        // GET: /Memo/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Memo memo = db.Frames.Find(id) as Memo;
            if (memo == null)
            {
                return View("Missing", new MissingItem(id));
            }

            this.FillTemplatesSelectList(db, FrameTypes.Memo, memo.TemplateId);
            
            return View(memo);
        }

        //
        // POST: /Memo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Memo memo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memo).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Frame");
            }


            this.FillTemplatesSelectList(db, FrameTypes.Memo, memo.TemplateId);
            
            return View(memo);
        }

        //
        // GET: /Memo/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Memo memo = db.Frames.Find(id) as Memo;
            if (memo == null)
            {
                return View("Missing", new MissingItem(id));
            }

            return View(memo);
        }

        //
        // POST: /Panel/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Frame frame = db.Frames.Find(id);
            db.Frames.Remove(frame);
            db.SaveChanges();

            return RedirectToAction("Index", "Frame");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}