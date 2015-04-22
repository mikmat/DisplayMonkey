﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisplayMonkey.Models;

using System.IO;

namespace DisplayMonkey.Controllers
{
    public class VideoController : BaseController
    {
        private DisplayMonkeyEntities db = new DisplayMonkeyEntities();

        //
        // GET: /Video/Details/5

        public ActionResult Details(int id = 0)
        {
            this.SaveReferrer(true);
            
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return View("Missing", new MissingItem(id));
            }
            return View(video);
        }

        //
        // GET: /Video/Create

        public ActionResult Create()
        {
            Frame frame = TempData[FrameController.SelectorFrameKey] as Frame;

            if (frame == null || frame.PanelId == 0)
            {
                return RedirectToAction("Create", "Frame");
            }

            Video video = new Video()
            {
                Frame = frame,
            };

            video.init(db);

            this.FillTemplatesSelectList(db, FrameTypes.Video);
            FillVideosSelectList();

            return View(video);
        }

        //
        // POST: /Video/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Video video, Frame frame)
        {
            video.Frame = frame;

            if (ModelState.IsValid)
            {
                if (video.SavedContentId.HasValue)
                {
                    Content content = db.Contents.Find(video.SavedContentId.Value);
                    video.Contents.Add(content);
                    db.Videos.Add(video);
                    db.SaveChanges();

                    return this.RestoreReferrer() ?? RedirectToAction("Index", "Frame");
                }
                else
                {
                    TempData["_newVideo"] = video;
                    return RedirectToAction("Upload", "Video");
                }
            }

            this.FillTemplatesSelectList(db, FrameTypes.Video, video.Frame.TemplateId);
            FillVideosSelectList();

            return View(video);
        }

        //
        // GET: /Picture/Upload

        public ActionResult Upload()
        {
            Video video = TempData["_newVideo"] as Video;

            if (video == null || video.Frame.PanelId == 0)
            {
                return RedirectToAction("Create", "Frame");
            }

            this.FillTemplatesSelectList(db, FrameTypes.Video, video.Frame.TemplateId);
            FillVideosSelectList();
            ViewBag.MaxVideoSize = Setting.GetSetting(db, Setting.Keys.MaxVideoSize).IntValuePositive;

            return View(video);
        }

        //
        // POST: /Picture/Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(Video video, Frame frame, IEnumerable<HttpPostedFileBase> files)
        {
            video.Frame = frame;
            
            // TODO: EditorFor

            bool hasFiles = false, addedFiles = false;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string ext = Path.GetExtension(file.FileName).Replace(".", "").ToUpper();
                    bool isVideo = Video.SupportedFormats.Contains(ext);

                    if (isVideo)
                    {
                        byte[] buffer = null;
                        using (BinaryReader reader = new BinaryReader(file.InputStream))
                        {
                            buffer = reader.ReadBytes(file.ContentLength);
                        }

                        Content content = new Content
                        {
                            Type = ContentTypes.ContentType_Video,
                            Name = Path.GetFileName(file.FileName),
                            Data = buffer,
                        };

                        video.Contents.Add(content);

                        addedFiles = true;
                    }

                    hasFiles = true;
                }
            }

            if (addedFiles)
            {
                db.Videos.Add(video);
                db.SaveChanges();

                return this.RestoreReferrer() ?? RedirectToAction("Index", "Frame");
            }

            else if (hasFiles)
            {
                // TODO: validator for wrong file types
            }

            this.FillTemplatesSelectList(db, FrameTypes.Video, video.Frame.TemplateId);
            FillVideosSelectList();
            ViewBag.MaxVideoSize = Setting.GetSetting(db, Setting.Keys.MaxVideoSize).IntValuePositive;

            return View(video);
        }

        //
        // GET: /Video/Link/5

        public ActionResult Link(int id = 0)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return View("Missing", new MissingItem(id));
            }

            this.FillTemplatesSelectList(db, FrameTypes.Video, video.Frame.TemplateId);
            FillAvailableVideosSelectList(id);

            return View(video);
        }

        //
        // POST: /Picture/Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Link(Video video)
        {
            if (video.SavedContentId > 0)
            {
                DisplayMonkey.Models.Content content = db.Contents.Find(video.SavedContentId);
                video = db.Videos.Find(video.FrameId);
                video.Contents.Add(content);
                db.SaveChanges();
                
                return this.RestoreReferrer() ?? RedirectToAction("Details", new { id = video.FrameId });
            }

            if (video.FrameId > 0)
            {
                return RedirectToAction("Uplink", new { id = video.FrameId });
            }
                
            return RedirectToAction("Index", "Frame");
        }

        //
        // GET: /Video/Uplink/5

        public ActionResult Uplink(int id = 0)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return View("Missing", new MissingItem(id));
            }

            return View(video);
        }

        //
        // POST: /Picture/Upload

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Uplink(Video video, IEnumerable<HttpPostedFileBase> files)
        {
            video = db.Videos.Find(video.FrameId);
            if (video == null)
            {
                return RedirectToAction("Index", "Frame");
            }

            // TODO: EditorFor

            bool hasFiles = false, addedFiles = false;
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string ext = Path.GetExtension(file.FileName).Replace(".", "").ToUpper();
                    bool isVideo = Video.SupportedFormats.Contains(ext);

                    if (isVideo)
                    {
                        byte[] buffer = null;
                        using (BinaryReader reader = new BinaryReader(file.InputStream))
                        {
                            buffer = reader.ReadBytes(file.ContentLength);
                        }

                        Content content = new Content
                        {
                            Type = ContentTypes.ContentType_Video,
                            Name = Path.GetFileName(file.FileName),
                            Data = buffer,
                        };

                        video.Contents.Add(content);

                        addedFiles = true;
                    }

                    hasFiles = true;
                }
            }

            if (addedFiles)
            {
                db.SaveChanges();
                
                return this.RestoreReferrer() ?? RedirectToAction("Details", new { id = video.FrameId });
            }

            else if (hasFiles)
            {
                // TODO: validator for wrong file types
            }

            return View(video);
        }

        //
        // GET: /Video/Unlink/5

        public ActionResult Unlink(int id = 0, int contentId = 0)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return View("Missing", new MissingItem(id));
            }

            Content content = db.Contents.Find(contentId);
            if (content == null)
            {
                return View("Missing", new MissingItem(id));
            }

            if (!video.Contents.Contains(content))
            {
                return View("Missing", new MissingItem(id));
            }

            ViewBag.Content = content;

            return View(video);
        }

        //
        // POST: /Video/Unlink/5

        [HttpPost, ActionName("Unlink")]
        [ValidateAntiForgeryToken]
        public ActionResult UnlinkConfirmed(int id, int contentId)
        {
            Video video = db.Videos.Find(id);
            Content content = db.Contents.Find(contentId);
            video.Contents.Remove(content);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        //
        // GET: /Video/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return View("Missing", new MissingItem(id));
            }

            this.FillTemplatesSelectList(db, FrameTypes.Video, video.Frame.TemplateId);

            return View(video);
        }

        //
        // POST: /Video/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Video video, Frame frame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(frame).State = EntityState.Modified;
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();

                return this.RestoreReferrer() ?? RedirectToAction("Index", "Frame");
            }

            video.Frame = frame;

            this.FillTemplatesSelectList(db, FrameTypes.Video, video.Frame.TemplateId);

            return View(video);
        }

        //
        // GET: /Video/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return View("Missing", new MissingItem(id));
            }
            return View(video);
        }

        //
        // POST: /Video/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Frame frame = db.Frames.Find(id);
            db.Frames.Remove(frame);
            db.SaveChanges();

            return this.RestoreReferrer(true) ?? RedirectToAction("Index", "Frame");
        }

        private void FillVideosSelectList(object selected = null)
        {
            var savedVideos = from m in db.Contents
                              where m.Type == ContentTypes.ContentType_Video
                              orderby m.Name
                              select m;

            ViewBag.Videos = new SelectList(savedVideos, "ContentId", "Name", selected);
        }

        private void FillAvailableVideosSelectList(int id = 0)
        {
            var savedVideos = from m in db.Contents
                              where m.Type == ContentTypes.ContentType_Video
                              && !db.Videos.Any(v => v.FrameId == id && v.Contents.Contains(m)) // exclude videos already linked
                              orderby m.Name
                              select m;

            ViewBag.Videos = new SelectList(savedVideos, "ContentId", "Name");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}