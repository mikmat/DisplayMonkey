using DisplayMonkey.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Web.Mvc;


using System.Data.Entity;
using System.Linq;
using System.Web;

using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Net;
using System.Text;
using System.Xml;
using DisplayMonkey.Language;



namespace DisplayMonkey.Controllers
{
    public class ScheduleController : BaseController
    {
        ApplicationDbContext context;

        public int ScheduleId { get; set; }
        public string ScheduleName { get;  set; }
        public TimeSpan StartTime { get;  set; }
        public TimeSpan EndTime { get;  set; }
        public bool Mon { get;  set; }
        public bool Tue { get;  set; }
        public bool Wed { get;  set; }
        public bool Thu { get;  set; }
        public bool Fri { get;  set; }
        public bool Sat { get;  set; }
        public bool Sun { get;  set; }

        public ScheduleController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Raspberry
        public ActionResult Index()
        {
            List<Schedule> schedules = new List<Schedule>();
            try
            {
                using (SqlCommand dispcmd = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandText = "SELECT * FROM Schedule ORDER BY ScheduleId"

                })
                {

                    dispcmd.ExecuteReaderExt((dispdr) =>
                    {
                        Schedule sched = new Schedule();

                        ScheduleId = dispdr.IntOrDefault("ScheduleId", 0);
                        ScheduleName = dispdr.StringOrBlank("ScheduleName");
                        StartTime = dispdr.TimeOrDefault("StartTime", new TimeSpan(0, 0, 0));
                        EndTime = dispdr.TimeOrDefault("EndTime", new TimeSpan(23, 59, 59));
                        Mon = dispdr.Boolean("Mon");
                        Tue = dispdr.Boolean("Tue");
                        Wed = dispdr.Boolean("Wed");
                        Thu = dispdr.Boolean("Thu");
                        Fri = dispdr.Boolean("Fri");
                        Sat = dispdr.Boolean("Sat");
                        Sun = dispdr.Boolean("Sun");

                        sched = new Schedule()
                        {
                            ScheduleId = ScheduleId,
                            ScheduleName = ScheduleName,
                            StartTime = StartTime,
                            EndTime = EndTime,
                            Mon = Mon,
                            Tue = Tue,
                            Wed = Wed,
                            Thu = Thu,
                            Fri = Fri,
                            Sat = Sat,
                            Sun = Sun
                        };

                        schedules.Add(sched);

 
                        while (dispdr.Read())
                        {
                            ScheduleId = dispdr.IntOrDefault("ScheduleId",0);
                            ScheduleName = dispdr.StringOrBlank("ScheduleName");
                            StartTime = dispdr.TimeOrDefault("StartTime",new TimeSpan(0,0,0));
                            EndTime = dispdr.TimeOrDefault("EndTime",new TimeSpan(23,59,59));
                            Mon = dispdr.Boolean("Mon");
                            Tue = dispdr.Boolean("Tue");
                            Wed = dispdr.Boolean("Wed");
                            Thu = dispdr.Boolean("Thu");
                            Fri = dispdr.Boolean("Fri");
                            Sat = dispdr.Boolean("Sat");
                            Sun = dispdr.Boolean("Sun");

                            sched = new Schedule()
                            {
                                ScheduleId = ScheduleId,
                                ScheduleName = ScheduleName,
                                StartTime = StartTime,
                                EndTime = EndTime,
                                Mon = Mon,
                                Tue = Tue, 
                                Wed = Wed,
                                Thu = Thu,
                                Fri = Fri,
                                Sat = Sat,
                                Sun = Sun
                            };

                            schedules.Add(sched);
                        }
                        return true;

                    });
                    
                }


                return View(schedules);
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
            return View(new Schedule());
        }

        // POST: Raspberry/Create
        [HttpPost]
        public ActionResult Create(Schedule schedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        try
                        {
                            using (SqlCommand updatecmd = new SqlCommand()
                            {
                                CommandType = CommandType.Text,
                                CommandText = "INSERT INTO Schedule(ScheduleName, StartTime, EndTime, Mon, Tue, Wed, Thu, Fri, Sat, Sun)" +
                                "VALUES('" + schedule.ScheduleName + "','"+schedule.StartTime + "','"+ schedule.EndTime
                                + "'," + Convert.ToInt32(schedule.Mon).ToString()
                                + "," + Convert.ToInt32(schedule.Tue).ToString() 
                                + "," + Convert.ToInt32(schedule.Wed).ToString() 
                                + "," + Convert.ToInt32(schedule.Thu).ToString()
                                + "," + Convert.ToInt32(schedule.Fri).ToString()
                                + "," + Convert.ToInt32(schedule.Sat).ToString() 
                                + "," + Convert.ToInt32(schedule.Sun).ToString()
                                + ")"
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

                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Raspberry/Edit/5
        public ActionResult Edit(int id)
        {
            Schedule schedule = new Schedule();
            try
            {
                using (SqlCommand dispcmd = new SqlCommand()
                {
                    CommandType = CommandType.Text,
                    CommandText = "SELECT top(1) * FROM Schedule where ScheduleId = " + id.ToString()

                })
                {

                    dispcmd.ExecuteReaderExt((dispdr) =>
                    {

                        ScheduleId = dispdr.IntOrDefault("ScheduleId", 0);
                        ScheduleName = dispdr.StringOrBlank("ScheduleName");
                        StartTime = dispdr.TimeOrDefault("StartTime", new TimeSpan(0, 0, 0));
                        EndTime = dispdr.TimeOrDefault("EndTime", new TimeSpan(23, 59, 59));
                        Mon = dispdr.Boolean("Mon");
                        Tue = dispdr.Boolean("Tue");
                        Wed = dispdr.Boolean("Wed");
                        Thu = dispdr.Boolean("Thu");
                        Fri = dispdr.Boolean("Fri");
                        Sat = dispdr.Boolean("Sat");
                        Sun = dispdr.Boolean("Sun");

                        schedule = new Schedule()
                        {
                            ScheduleId = ScheduleId,
                            ScheduleName = ScheduleName,
                            StartTime = StartTime,
                            EndTime = EndTime,
                            Mon = Mon,
                            Tue = Tue,
                            Wed = Wed,
                            Thu = Thu,
                            Fri = Fri,
                            Sat = Sat,
                            Sun = Sun
                        };
                        return true;
                    });
                }

                return View(schedule);
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        // POST: Raspberry/Edit/5
        [HttpPost]
        public ActionResult Edit(Schedule schedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        try
                        {
                            using (SqlCommand updatecmd = new SqlCommand()
                            {
                                CommandType = CommandType.Text,
                                CommandText = "UPDATE Schedule SET ScheduleName='" + schedule.ScheduleName + "', StartTime='" + schedule.StartTime + "',EndTime='" 
                                + schedule.EndTime + "',Mon=" + Convert.ToInt32(schedule.Mon).ToString()
                                + ",Tue=" + Convert.ToInt32(schedule.Tue).ToString() + ",Wed=" + Convert.ToInt32(schedule.Wed).ToString() + ",Thu=" + Convert.ToInt32(schedule.Thu).ToString() 
                                + ",Fri=" + Convert.ToInt32(schedule.Fri).ToString() + ",Sat=" + Convert.ToInt32(schedule.Sat).ToString() + ",Sun=" + Convert.ToInt32(schedule.Sun).ToString()
                                + " WHERE ScheduleId=" + schedule.ScheduleId.ToString()
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

                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Raspberry/Delete/5
        public ActionResult Delete(int id)
        {
            Schedule schedule = new Schedule();
            using (SqlCommand dispcmd = new SqlCommand()
            {
                CommandType = CommandType.Text,
                CommandText = "SELECT top(1) * FROM Schedule where ScheduleId = " + id.ToString()

            })
            {
                
                dispcmd.ExecuteReaderExt((dispdr) =>
                {

                    ScheduleId = dispdr.IntOrDefault("ScheduleId", 0);
                    ScheduleName = dispdr.StringOrBlank("ScheduleName");
                    StartTime = dispdr.TimeOrDefault("StartTime", new TimeSpan(0, 0, 0));
                    EndTime = dispdr.TimeOrDefault("EndTime", new TimeSpan(23, 59, 59));
                    Mon = dispdr.Boolean("Mon");
                    Tue = dispdr.Boolean("Tue");
                    Wed = dispdr.Boolean("Wed");
                    Thu = dispdr.Boolean("Thu");
                    Fri = dispdr.Boolean("Fri");
                    Sat = dispdr.Boolean("Sat");
                    Sun = dispdr.Boolean("Sun");

                    schedule = new Schedule()
                    {
                        ScheduleId = ScheduleId,
                        ScheduleName = ScheduleName,
                        StartTime = StartTime,
                        EndTime = EndTime,
                        Mon = Mon,
                        Tue = Tue,
                        Wed = Wed,
                        Thu = Thu,
                        Fri = Fri,
                        Sat = Sat,
                        Sun = Sun
                    };
                    return true;
                });
            }

            return View(schedule);

        }

        // POST: Raspberry/Delete/5
        [HttpPost]
        public ActionResult Delete(Schedule schedule)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        try
                        {
                            using (SqlCommand updatecmd = new SqlCommand()
                            {
                                CommandType = CommandType.Text,
                                CommandText = "DELETE FROM Schedule WHERE ScheduleId=" + schedule.ScheduleId.ToString()
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

                        try
                        {
                            using (SqlCommand updatecmd = new SqlCommand()
                            {
                                CommandType = CommandType.Text,
                                CommandText = "DELETE FROM FrameSchedule WHERE ScheduleId=" + schedule.ScheduleId.ToString()
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


                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }
                }
                else
                {
                    return View(schedule);
                }
            }
            catch
            {
                return View();
            }
          
        }
    }
}
