﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisplayMonkey.Controllers
{
    public class HelpController : Controller
    {
        //
        // GET: /Help
        // GET: /Help/Source/Page
        // GET: /Help/Source/Page/id
        public ActionResult Index(string source = "Home", string page = "Index", int id = 0)
        {
            string helpPath = "~/doc/";
            if (source != "")
            {
                helpPath += string.Format("{0}/{1}.html", source, page);
            }
            else
            {
                helpPath += "home.html";
            }
            return File(helpPath, "text/html");
        }
	}
}