using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisplayMonkey.Models;

namespace DisplayMonkey.Controllers
{
    public class TemplogController : Controller
    {
        ApplicationDbContext context;
        public TemplogController()
        {
            context = new ApplicationDbContext();
        }


        // GET: Templog
        public ActionResult Index()
        {
            return View();
        }

        // GET: Templog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Templog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Templog/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Templog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Templog/Edit/5
        [HttpPost]
        public void Edit(string ip, string hostname, string temperature, string mac)
        {
            try
            {
                try
                {
                    using (SqlCommand updatecmd = new SqlCommand()
                    {
                        CommandType = CommandType.Text,
                        CommandText = "EXEC [SP_UpdatePiStatus] '"+mac+"', '"+hostname+ "','" + ip + "', '" + temperature+"'"
                        

                    })
                    {
                        updatecmd.ExecuteNonQueryExt();

                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex);
                    throw;
                }
                //var temprec = from g in context.Roles

                return; // RedirectToAction("Index");
            }
            catch
            {
                return;// View();
            }
        }

        // GET: Templog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Templog/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
