using ECMS.Models.Database;
using ECMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Services
{
    public class GlobalDataService
    {
        public string SiteTitle { get; set; }
        public string Logo { get; set; }
        public List<CategoryModel> CategoryList { get; set; }
        public List<CartItem> CartItems { get; set; }
        public int CartTotal { get; set; }
        public int DiscountAmount { get; set; }
        public string DiscountCoupon { get; set; }
        public GlobalDataService()
        {
            DBEntities db = new DBEntities();
            ShoppingCartService _shoppingCartService = new ShoppingCartService(System.Web.HttpContext.Current);

            var generalSetting = db.GeneralSettings.FirstOrDefault(gs => gs.status);
            SiteTitle = generalSetting.name;
            Logo = generalSetting.logo;

            var categories = db.Categories.Where(category=> category.status==true).ToList();
            var categoryList = new List<CategoryModel>();
            foreach(var category in categories)
            {
                CategoryModel categoryModel = new CategoryModel();
                categoryModel.id = category.id;
                categoryModel.name = category.name;
                categoryModel.slug = category.slug;
                categoryModel.description = category.description;
                categoryModel.front_view = category.front_view;
                categoryModel.image = category.image;
                categoryModel.status = category.status;
                categoryList.Add(categoryModel);
            }
            CategoryList = categoryList;

            CartItems = _shoppingCartService.GetItems();
            CartTotal = _shoppingCartService.GetTotal();
            DiscountAmount = _shoppingCartService.GetDiscountAmount();
            DiscountCoupon = _shoppingCartService.GetDiscountCoupon();

        }
    }
}