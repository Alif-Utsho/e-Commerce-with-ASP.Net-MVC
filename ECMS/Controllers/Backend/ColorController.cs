using ECMS.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers.Backend
{

    public class ColorController : AdminBaseController
    {
        public ActionResult Index()
        {
            var colors = _dbContext.Colors.ToList();
            return View(colors);
        }
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Upload()
        {
            var colorName = Request.Form["colorName"];
            var colorCode = Request.Form["color"];
            var color = new Color
            {
                colorName = colorName,
                color1 = colorCode,
                status = true
            };
            _dbContext.Colors.Add(color);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Inserted successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var color = _dbContext.Colors.FirstOrDefault(i => i.Id == id);
            return View(color);
        }

        public ActionResult Update()
        {
            var id = int.Parse(Request.Form["id"]);
            var color = _dbContext.Colors.FirstOrDefault(i => i.Id == id);
            color.colorName = Request.Form["colorName"];
            color.color1 = Request.Form["color"];
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Updated successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var color = _dbContext.Colors.FirstOrDefault(i => i.Id == id);

            var dependentRecords = _dbContext.ProductColors.Where(ps => ps.color_id == id).ToList();
            _dbContext.ProductColors.RemoveRange(dependentRecords);

            _dbContext.Colors.Remove(color);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}