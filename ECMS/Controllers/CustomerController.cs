using ECMS.Models.Database;
using ECMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController(GlobalDataService globalDataService, DBEntities dbContext) : base(globalDataService, dbContext) { }

        public ActionResult Index()
        {
            return View();
        }
    }
}