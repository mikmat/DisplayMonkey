﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisplayMonkey.Models;

namespace DisplayMonkey.Controllers
{
    public class ReportServerController : Controller
    {
        private DisplayMonkeyEntities db = new DisplayMonkeyEntities();

        //
        // GET: /ReportServer/

        public ActionResult Index()
        {
            return View(db.ReportServers.ToList());
        }

        //
        // GET: /ReportServer/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ReportServer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReportServer reportserver)
        {
            if (ModelState.IsValid)
            {
                db.ReportServers.Add(reportserver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportserver);
        }

        //
        // GET: /ReportServer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ReportServer reportserver = db.ReportServers.Find(id);
            if (reportserver == null)
            {
                return HttpNotFound();
            }
            return View(reportserver);
        }

        //
        // POST: /ReportServer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReportServer reportserver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportserver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportserver);
        }

        //
        // GET: /ReportServer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ReportServer reportserver = db.ReportServers.Find(id);
            if (reportserver == null)
            {
                return HttpNotFound();
            }
            return View(reportserver);
        }

        //
        // POST: /ReportServer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportServer reportserver = db.ReportServers.Find(id);
            db.ReportServers.Remove(reportserver);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}