using ECMS.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Services
{
    public class ShoppingCartService
    {
        private const string CartCookieName = "ShoppingCart";
        private readonly HttpContext _context;
        private readonly HttpSessionStateBase _session;

        public ShoppingCartService(HttpContext context)
        {
            _context = context;
            _session = new HttpSessionStateWrapper(context.Session);
        }

        public void AddItem(CartItem cartItem)
        {
            var items = GetItems().ToList();
            var existingItem = items.FirstOrDefault(i => i.ProductId == cartItem.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            else
            {
                items.Add(cartItem);
            }
            SaveItems(items);
        }

        public void RemoveItem(int productId)
        {
            var items = GetItems().ToList();
            items.RemoveAll(i => i.ProductId == productId);
            SaveItems(items);
        }

        public void ClearCart()
        {
            SaveItems(new List<CartItem>());
        }

        public List<CartItem> GetItems()
        {
            var cookie = _context.Request.Cookies[CartCookieName];
            if (cookie != null)
            {
                var json = cookie.Value;
                return JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
            }
            return new List<CartItem>();
        }

        public int GetTotal()
        {
            return GetItems().Sum(i => i.Price * i.Quantity);
        }

        public void SaveItems(List<CartItem> items)
        {
            var json = JsonConvert.SerializeObject(items);
            var cookie = new HttpCookie(CartCookieName, json)
            {
                Expires = DateTime.Now.AddDays(30)
            };
            _context.Response.Cookies.Add(cookie);
        }

        public int GetDiscountAmount()
        {
            if (_session["DiscountAmount"] != null)
            {
                if (int.TryParse(_session["DiscountAmount"].ToString(), out int discountAmount))
                {
                    return discountAmount;
                }
            }
            return 0; 
        }

        public string GetDiscountCoupon()
        {
            if (_session["CouponCode"] != null)
            {
                return _session["CouponCode"].ToString();
            }
            return null;
        }
    }
}