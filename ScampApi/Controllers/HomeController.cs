<<<<<<< HEAD
﻿using Microsoft.AspNet.Mvc;
using Microsoft.Framework.ConfigurationModel;
using ScampApi.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ScampApi.Controllers
{
    public class HomeController : Controller
    {

        IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var settings = new ScampSettings
            {
                TenantId = _config.Get("TenantId"),
                ClientId = _config.Get("ClientId")
            };

            return View(settings);
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

namespace Scamp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
>>>>>>> master
