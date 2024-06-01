using ECMS.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers.Backend
{
    public class GeneralsettingController : AdminBaseController
    {
        public ActionResult Index()
        {
            var generalsetting = _dbContext.GeneralSettings.FirstOrDefault();
            return View(generalsetting);
        }
    }
}