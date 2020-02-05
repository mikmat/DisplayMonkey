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
using System.IO;
using System.Linq;
//using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DisplayMonkey.Models;
using file;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace DisplayMonkey.Controllers
{
    public class HtmlController : BaseController
    {
        private DisplayMonkeyEntities db = new DisplayMonkeyEntities();

        // GET: /Html/Details/5
        public ActionResult Details(int id = 0)
        {
            Html html = db.Frames.Find(id) as Html;
            if (html == null)
            {
                return View("Missing", new MissingItem(id));
            }
            return View(html);
        }

        // GET: /Html/Create
        public ActionResult Create()
        {
            Frame frame = TempData[FrameController.SelectorFrameKey] as Frame;

            if (frame == null || frame.PanelId == 0)
            {
                return RedirectToAction("Create", "Frame");
            }

            Html html = new Html(frame, db)
            {
                Panel = db.Panels
                    .Include(p => p.Canvas)
                    .FirstOrDefault(p => p.PanelId == frame.PanelId),
            };

            this.FillTemplatesSelectList(db, FrameTypes.Html);
            
            return View(html);
        }

        // POST: /Html/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Html html)
        {
            if (ModelState.IsValid)
            {
                db.Frames.Add(html);
                db.SaveChanges();

                // Om användaren är i rollen "butik" ska vi skapa en post i FrameLocation
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

            html.Panel = db.Panels
                .Include(p => p.Canvas)
                .FirstOrDefault(p => p.PanelId == html.PanelId)
                ;

            this.FillTemplatesSelectList(db, FrameTypes.Html, html.TemplateId);
            
            return View(html);
        }

        // GET: /Html/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Html html = db.Frames.Find(id) as Html;
            if (html == null)
            {
                return View("Missing", new MissingItem(id));
            }

            this.FillTemplatesSelectList(db, FrameTypes.Html, html.TemplateId);

            return View(html);
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase file)
        {
            var uploadsPath = HostingEnvironment.MapPath($"/uploads");
            var uploadsDir = new DirectoryInfo(uploadsPath);
            if (!uploadsDir.Exists)
                uploadsDir.Create();

            var imageRelativePath = $"/uploads/{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{file.FileName}";
            var imageAbsPath = HostingEnvironment.MapPath(imageRelativePath);
            var tjo = HostingEnvironment.ApplicationVirtualPath;
            var imageBytes = file.InputStream.ReadToEnd();
            System.IO.File.WriteAllBytes(imageAbsPath, imageBytes);

            var request = HttpContext.Request; 
            var address = string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority);

            return Json(new { location = address+imageRelativePath });

        }


        // POST: /Html/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Html html)
        {
            if (ModelState.IsValid)
            {
                db.Entry(html).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Frame");
            }


            this.FillTemplatesSelectList(db, FrameTypes.Html, html.TemplateId);
            
            return View(html);
        }

        // GET: /Html/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Html html = db.Frames.Find(id) as Html;
            if (html == null)
            {
                return View("Missing", new MissingItem(id));
            }

            return View(html);
        }

        //
        // GET: /Media/Preview/5

        //[Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Preview(int id)
        {
            Html html = db.Frames.Find(id) as Html;
            if (html == null)
            {
                return HttpNotFound();
            }

            return Content(html.Content);
        }

        // POST: /Html/Delete/5
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
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

namespace file
{
    public static class InputStream
    {
        public static byte[] ReadToEnd(this Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}