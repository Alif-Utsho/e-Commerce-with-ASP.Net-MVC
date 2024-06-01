using ECMS.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers.Backend
{
    public class SizeController : AdminBaseController
    {
        public ActionResult Index()
        {
            var sizes = _dbContext.Sizes.ToList();
            return View(sizes);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Upload()
        {
            var sizeName = Request.Form["sizeName"];
            var size = new Size
            {
                sizeName = sizeName,
                status = new byte[50]
            };
            _dbContext.Sizes.Add(size);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Inserted successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var size = _dbContext.Sizes.FirstOrDefault(i => i.id == id);
            return View(size);
        }

        public ActionResult Update()
        {
            var id = int.Parse(Request.Form["id"]);
            var size = _dbContext.Sizes.FirstOrDefault(i => i.id == id);
            size.sizeName = Request.Form["sizeName"];            
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Updated successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var size = _dbContext.Sizes.FirstOrDefault(i => i.id == id);

            var dependentRecords = _dbContext.ProductSizes.Where(ps => ps.size_id == id).ToList();
            _dbContext.ProductSizes.RemoveRange(dependentRecords);

            _dbContext.Sizes.Remove(size);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}