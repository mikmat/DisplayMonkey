using DisplayMonkey.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisplayMonkey.Controllers
{
    public class RaspberryController : Controller
    {
        ApplicationDbContext context;

        public string mac { get; private set; }
        public string hostname { get; private set; }
        public string ip { get; private set; }
        public DateTime updated { get; private set; }
        public decimal temperature { get; private set; }
        public DateTime firstseen { get; private set; }
        public Int32 disabled { get; private set; }
        public DateTime rebooted { get; private set; }

        public RaspberryController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Raspberry
        public ActionResult Index()
        {
            List<Raspberry> raspberries = new List<Raspberry>();
            try
            {
                Response.AddHeader("Refresh", "60");
                using (SqlCommand dispcmd = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM RaspberryList where disabled = 0 AND hostname like 'mtv%'  ORDER BY updated, hostname",

                })
                {

                    dispcmd.ExecuteReaderExt((dispdr) =>
                    {
                        mac = dispdr.StringOrBlank("mac");
                        hostname = dispdr.StringOrBlank("hostname");
                        ip = dispdr.StringOrBlank("ip");
                        updated = dispdr.DateTimeOrBlank("updated");
                        temperature = dispdr.DecimalOrZero("temperature");
                        firstseen = dispdr.DateTimeOrBlank("firstseen");
                        disabled = dispdr.IntOrZero("disabled");
                        rebooted = dispdr.DateTimeOrBlank("uptime");

                        Raspberry rasp = new Raspberry
                        {
                            mac = mac,
                            hostname = hostname,
                            ip = ip,
                            updated = updated,
                            temperature = temperature,
                            firstseen = firstseen,
                            disabled = disabled, 
                            rebooted = rebooted
                        };

                        raspberries.Add(rasp);
                        while (dispdr.Read())
                        {
                            mac = dispdr.StringOrBlank("mac");
                            hostname = dispdr.StringOrBlank("hostname");
                            ip = dispdr.StringOrBlank("ip");
                            updated = dispdr.DateTimeOrBlank("updated");
                            temperature = dispdr.DecimalOrZero("temperature");
                            firstseen = dispdr.DateTimeOrBlank("firstseen");
                            disabled = dispdr.IntOrZero("disabled");
                            rebooted = dispdr.DateTimeOrBlank("uptime");

                            rasp = new Raspberry
                            {
                                mac = mac,
                                hostname = hostname,
                                ip = ip,
                                updated = updated,
                                temperature = temperature,
                                firstseen = firstseen,
                                disabled = disabled,
                                rebooted = rebooted
                            };

                            raspberries.Add(rasp);
                        }
                        return true;

                    });
                    
                }

                return View(raspberries);
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        public ActionResult Queue()
        {
            List<Raspberry> raspberries = new List<Raspberry>();
            try
            {
                Response.AddHeader("Refresh", "60");
                string sqlstring = "SELECT * FROM RaspberryList where disabled = 0 AND hostname like 'queue%' ORDER BY updated, hostname";
                
                using (SqlCommand dispcmd = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandText = sqlstring,

                })
                {

                    dispcmd.ExecuteReaderExt((dispdr) =>
                    {
                        mac = dispdr.StringOrBlank("mac");
                        hostname = dispdr.StringOrBlank("hostname");
                        ip = dispdr.StringOrBlank("ip");
                        updated = dispdr.DateTimeOrBlank("updated");
                        temperature = dispdr.DecimalOrZero("temperature");
                        firstseen = dispdr.DateTimeOrBlank("firstseen");
                        disabled = dispdr.IntOrZero("disabled");
                        rebooted = dispdr.DateTimeOrBlank("uptime");

                        Raspberry rasp = new Raspberry
                        {
                            mac = mac,
                            hostname = hostname,
                            ip = ip,
                            updated = updated,
                            temperature = temperature,
                            firstseen = firstseen,
                            disabled = disabled,
                            rebooted = rebooted
                        };

                        raspberries.Add(rasp);
                        while (dispdr.Read())
                        {
                            mac = dispdr.StringOrBlank("mac");
                            hostname = dispdr.StringOrBlank("hostname");
                            ip = dispdr.StringOrBlank("ip");
                            updated = dispdr.DateTimeOrBlank("updated");
                            temperature = dispdr.DecimalOrZero("temperature");
                            firstseen = dispdr.DateTimeOrBlank("firstseen");
                            disabled = dispdr.IntOrZero("disabled");
                            rebooted = dispdr.DateTimeOrBlank("uptime");

                            rasp = new Raspberry
                            {
                                mac = mac,
                                hostname = hostname,
                                ip = ip,
                                updated = updated,
                                temperature = temperature,
                                firstseen = firstseen,
                                disabled = disabled,
                                rebooted = rebooted
                            };

                            raspberries.Add(rasp);
                        }
                        return true;

                    });

                }

                return View(raspberries);
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        // GET: Raspberry/Details/5
        public ActionResult Details(int id)
        { 
            return View();
        }

        // GET: Raspberry/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Raspberry/Create
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

        // GET: Raspberry/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Raspberry/Edit/5
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

        // GET: Raspberry/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Raspberry/Delete/5
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
    /*
    public class raspberry
    {
        public string mac { get;  set; }
        public string hostname { get;  set; }
        public string ip { get;  set; }
        public DateTime updated { get;  set; }
        public decimal temperature { get;  set; }
        public DateTime firstseen { get;  set; }
        public Int32 disabled { get;  set; }
    }*/
}
