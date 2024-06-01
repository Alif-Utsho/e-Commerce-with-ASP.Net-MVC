using AutoMapper;
using ECMS.Models.Database;
using ECMS.Models.Entities;
using ECMS.Models.RequestModel;
using Slugify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers.Backend
{
    public class CategoryController : AdminBaseController
    {
        public ActionResult Index()
        {
            var categoryLists = _dbContext.Categories.ToList();
            var config_category = new MapperConfiguration(c => c.CreateMap<Category, CategoryModel>());
            var mapper_category = new Mapper(config_category);
            var categories = mapper_category.Map<List<CategoryModel>>(categoryLists);
            return View(categories);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Upload(CategoryRequestModel categoryRequest)
        {
            var imageName = Path.GetFileName(Request.Files[0].FileName);
            var imagePath = Path.Combine(Server.MapPath("~/Assets/Uploads/"), imageName);
            Request.Files[0].SaveAs(imagePath);
            
            var slugHelper = new SlugHelper();

            var categoryModel = new Category
            {
                name = categoryRequest.name,
                slug = slugHelper.GenerateSlug(categoryRequest.name),
                description = categoryRequest.description,
                image = "~/Assets/Uploads/" + imageName,
                front_view = categoryRequest.front_view,
                status = true
            };

            _dbContext.Categories.Add(categoryModel);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data inserted successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var categoryDB = _dbContext.Categories.FirstOrDefault(i => i.id == id);
            var config_category = new MapperConfiguration(c => c.CreateMap<Category, CategoryModel>());
            var mapper_category = new Mapper(config_category);
            var category = mapper_category.Map<CategoryModel>(categoryDB);

            return View(category);
        }

        public ActionResult Update(CategoryRequestModel categoryRequest)
        {
            var slugHelper = new SlugHelper();

            var category = _dbContext.Categories.FirstOrDefault(i => i.id == categoryRequest.id);
            category.name = categoryRequest.name;
            category.slug = slugHelper.GenerateSlug(categoryRequest.name);
            category.description = categoryRequest.description;
            category.front_view = categoryRequest.front_view;
            category.status = true;

            if (Request.Files != null && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
            {
                var imageName = Path.GetFileName(Request.Files[0].FileName);
                var filePath = Path.Combine(Server.MapPath("~/Assets/Uploads/"), imageName);
                Request.Files[0].SaveAs(filePath);
                category.image = "~/Assets/Uploads/" + imageName;
            }

            _dbContext.SaveChanges();

            TempData["Success"] = "Data updated successfully";
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(i => i.id == id);

            var products = _dbContext.Products.Where(p => p.category_id == id);
            var productIds = products.Select(p => p.id);
            
            // Images remove
            var productImages = _dbContext.ProductImages.Where(pi => productIds.Contains(pi.product_id));
            _dbContext.ProductImages.RemoveRange(productImages);
            // Sizes remove
            var productSizes = _dbContext.ProductSizes.Where(ps => productIds.Contains(ps.product_id));
            _dbContext.ProductSizes.RemoveRange(productSizes);
            // Colors remove
            var productColors = _dbContext.ProductColors.Where(pc => productIds.Contains(pc.product_id));
            _dbContext.ProductColors.RemoveRange(productColors);
            // Product remove
            _dbContext.Products.RemoveRange(products);

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();

            TempData["Success"] = "Data deleted successfully";
            return RedirectToAction("Index");
        }
    }
}