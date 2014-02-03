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
    //[HandleError]
    public class LevelController : Controller
    {
        private DisplayMonkeyEntities db = new DisplayMonkeyEntities();

        //[HandleError]
        //public ActionResult ThrowException()
        //{
        //    throw new ApplicationException();
        //}

        //
        // GET: /Level/

        public ActionResult Index()
        {
            Navigation.SaveCurrent();
            return View(db.Levels.ToList());
        }

        //
        // GET: /Level/Details/5

        public ActionResult Details(int id = 0)
        {
            Navigation.SaveCurrent();
            
            Level level = db.Levels.Find(id);
            if (level == null)
            {
                return HttpNotFound();
            }

            level.Locations = db.Locations
                               .Where(l => l.LevelId == id)
                               .ToList();

            return View(level);
        }

        //
        // GET: /Level/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Level/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Level level)
        {
            if (ModelState.IsValid)
            {
                db.Levels.Add(level);
                db.SaveChanges();

                Navigation.Restore();

                return RedirectToAction("Index");
            }

            return View(level);
        }

        //
        // GET: /Level/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Level level = db.Levels.Find(id);
            if (level == null)
            {
                return HttpNotFound();
            }
            return View(level);
        }

        //
        // POST: /Level/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Level level)
        {
            if (ModelState.IsValid)
            {
                db.Entry(level).State = EntityState.Modified;
                db.SaveChanges();

                Navigation.Restore();

                return RedirectToAction("Index");
            }
            return View(level);
        }

        //
        // GET: /Level/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Level level = db.Levels.Find(id);
            if (level == null)
            {
                return HttpNotFound();
            }
            return View(level);
        }

        //
        // POST: /Level/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Level level = db.Levels.Find(id);
            if (level == null)
            {
                return HttpNotFound();
            }
            db.Levels.Remove(level);
            db.SaveChanges();

            Navigation.Restore();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}