using AutoMapper;
using ECMS.Models.Database;
using ECMS.Models.Entities;
using ECMS.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Slugify;
using System.Web.Mvc;
using System.IO;

namespace ECMS.Controllers.Backend
{
    public class ProductController : AdminBaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            var productLists = _dbContext.Products.ToList();
            var config_categoryProducts = new MapperConfiguration(c => c.CreateMap<Product, ProductModel>());
            var mapper_categoryProducts = new Mapper(config_categoryProducts);
            var products = mapper_categoryProducts.Map<List<ProductModel>>(productLists);
            return View(products);
        }

        public ActionResult GetListByBrand(string brand)
        {
            var productList = _dbContext.Products.Where(i => i.brand.Equals(brand)).ToList();
            return View();


        }

        public ActionResult Create()
        {
            var categoryLists = _dbContext.Categories.ToList();
            var config_category = new MapperConfiguration(c => c.CreateMap<Category, CategoryModel>());
            var mapper_category = new Mapper(config_category);
            var categories = mapper_category.Map<List<CategoryModel>>(categoryLists);

            var colors = _dbContext.Colors.ToList();
            var sizes = _dbContext.Sizes.ToList();

            ViewBag.Categories = categories;
            ViewBag.Colors = colors;
            ViewBag.Sizes = sizes;
            return View();
        }

        [HttpPost]
        public ActionResult Upload(ProductRequestModel productRequest)
        {
            /*if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all the required fields.";
                return RedirectToAction("Create");
            }*/
            var slugHelper = new SlugHelper();

            var thumbnailName = Path.GetFileName(Request.Files[0].FileName);
            var thumbnailPath = Path.Combine(Server.MapPath("~/Assets/Uploads/"), thumbnailName);
            Request.Files[0].SaveAs(thumbnailPath);

            int discountInt = Convert.ToInt32(productRequest.discount);

            var product = new Product
            {
                name = productRequest.name,
                slug = slugHelper.GenerateSlug(productRequest.name),
                category_id = productRequest.category_id,
                description = productRequest.description,
                additional_description = productRequest.additional_description,
                new_price = productRequest.new_price,
                old_price = productRequest.old_price,
                brand = productRequest.brand,
                discount = discountInt,
                purchase_price = productRequest.purchase_price,
                stock = productRequest.stock,
                thumbnail = "/Assets/Uploads/" + thumbnailName,
                status = true,
                sold = 0
            };
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            for (int i = 1; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Assets/Uploads/"), fileName);
                    file.SaveAs(path);

                    var productImage = new ProductImage
                    {
                        product_id = product.id,
                        image = "/Assets/Uploads/" + fileName
                    };
                    _dbContext.ProductImages.Add(productImage);
                }
            }
            _dbContext.SaveChanges();

            foreach (var size in productRequest.sizes)
            {
                var productsize = new ProductSize
                {
                    product_id = product.id,
                    size_id = size
                };
                _dbContext.ProductSizes.Add(productsize);
            }
            _dbContext.SaveChanges();

            foreach (var color in productRequest.colors)
            {
                var productsize = new ProductColor
                {
                    product_id = product.id,
                    color_id = color
                };
                _dbContext.ProductColors.Add(productsize);
            }
            _dbContext.SaveChanges();

            TempData["Success"] = "A New Product Uploaded.";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var productItem = _dbContext.Products.FirstOrDefault(i=>i.id==id);
            var config_categoryProduct = new MapperConfiguration(c => c.CreateMap<Product, ProductDetailsModel>());
            var mapper_categoryProduct = new Mapper(config_categoryProduct);
            var productDetails = mapper_categoryProduct.Map<ProductDetailsModel>(productItem);

            var categoryLists = _dbContext.Categories.ToList();
            var config_category = new MapperConfiguration(c => c.CreateMap<Category, CategoryModel>());
            var mapper_category = new Mapper(config_category);
            var categories = mapper_category.Map<List<CategoryModel>>(categoryLists);

            var colors = _dbContext.Colors.ToList();
            var sizes = _dbContext.Sizes.ToList();

            ViewBag.Categories = categories;
            ViewBag.Colors = colors;
            ViewBag.Sizes = sizes;

            return View(productDetails);
        }

        public ActionResult LowStock()
        {
            var productLists = _dbContext.Products.Where(i=>i.stock<=5).ToList();
            var config_categoryProducts = new MapperConfiguration(c => c.CreateMap<Product, ProductModel>());
            var mapper_categoryProducts = new Mapper(config_categoryProducts);
            var products = mapper_categoryProducts.Map<List<ProductModel>>(productLists);
            return View(products);
        }

    }
}