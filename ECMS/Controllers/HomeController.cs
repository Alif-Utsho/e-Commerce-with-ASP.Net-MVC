using AutoMapper;
using ECMS.Models.Database;
using ECMS.Models.Entities;
using ECMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(GlobalDataService globalDataService, DBEntities dbContext) : base(globalDataService, dbContext) { }

        public ActionResult Index()
        {
            var banners = _dbContext.Banners.Where(banner=> banner.status == true).ToList();
            ViewBag.banners = banners;

            var front_categories = _dbContext.Categories.Where(category => category.front_view == true).ToList();
            var config_category = new MapperConfiguration(c => c.CreateMap<Category, CategoryModel>());
            var mapper_category = new Mapper(config_category);
            var frontCategoryModels = mapper_category.Map<List<CategoryModel>>(front_categories);
            ViewBag.FrontCategories = frontCategoryModels;

            var config_categoryProducts = new MapperConfiguration(c => c.CreateMap<Category, CategoryProductModel>());
            var mapper_categoryProducts = new Mapper(config_categoryProducts);
            var products = mapper_categoryProducts.Map<List<CategoryProductModel>>(front_categories);
            ViewBag.CategoryProducts = products;

            return View();
        }

        public ActionResult ProductDetails(string id)
        {
            var product = _dbContext.Products.Where(p => p.slug.Equals(id)).FirstOrDefault();
            if (product == null)
            {
                return RedirectToAction("NotFound");
            }
            var config_product = new MapperConfiguration(c => c.CreateMap<Product, ProductDetailsModel>());
            var mapper_product = new Mapper(config_product);
            var productDetails = mapper_product.Map<ProductDetailsModel>(product);

            var products = _dbContext.Products.Where(p => p.category_id == productDetails.category_id && p.id != productDetails.id).ToList();
            var config_relatedproduct = new MapperConfiguration(c => c.CreateMap<Product, ProductModel>());
            var mapper_relatedproduct = new Mapper(config_relatedproduct);
            var relatedProducts = mapper_relatedproduct.Map<List<ProductModel>>(products);

            ViewBag.RelatedProducts = relatedProducts;
            return View(productDetails);

        }

        public ActionResult Category(string id)
        {
            var category = _dbContext.Categories.Where(cat => cat.slug.Equals(id)).FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("NotFound");
            }
            var config_categoryProducts = new MapperConfiguration(c => c.CreateMap<Category, CategoryProductModel>());
            var mapper_categoryProducts = new Mapper(config_categoryProducts);
            var products = mapper_categoryProducts.Map<CategoryProductModel>(category);
            return View(products);
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}