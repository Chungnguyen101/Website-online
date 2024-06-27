using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DATN2.Models;
using DATN2.Models.EF;

namespace DATN2.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }
    }
}