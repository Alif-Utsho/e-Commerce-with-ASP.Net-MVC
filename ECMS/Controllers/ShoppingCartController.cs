using ECMS.Models.Database;
using ECMS.Models.Entities;
using ECMS.Models.RequestModel;
using ECMS.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECMS.Controllers
{
    public class ShoppingCartController : BaseController
    {

        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(GlobalDataService globalDataService, DBEntities dbContext) : base(globalDataService, dbContext)
    {
            _shoppingCartService = new ShoppingCartService(System.Web.HttpContext.Current);
        }

        public ActionResult Index()
        {
            var shoppingCart = _shoppingCartService.GetItems();
            ViewBag.CartTotal = _shoppingCartService.GetTotal();
            ViewBag.DiscountAmount = _shoppingCartService.GetDiscountAmount();
            ViewBag.DiscountCoupon = _shoppingCartService.GetDiscountCoupon();

            return View(shoppingCart);
        }

        [HttpPost]
        public ActionResult AddToCart()
        {
            int productId = int.Parse(Request.Form["productId"]);
            int quantity = int.Parse(Request.Form["quantity"]);
            string color = Request.Form["color"] ?? null;
            string size = Request.Form["size"] ?? null;

            var cartItem = GetProductById(productId, quantity, color, size);
            if (cartItem != null)
            {
                _shoppingCartService.AddItem(cartItem);
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            _shoppingCartService.RemoveItem(id);
            return RedirectToAction("Index");
        }

        private CartItem GetProductById(int productId, int quantity, string color, string size)
        {
            var product = _dbContext.Products.FirstOrDefault(i => i.id == productId);
            var cartItem = new CartItem
            {
                ProductId = product.id,
                ProductName = product.name,
                Price = product.new_price,
                Quantity = quantity,
                ProductColor = color,
                ProductSize = size,
                ProductImage = product.thumbnail
            };
            return cartItem;
        }

        [HttpPost]
        public ActionResult UpdateCart(int[] ProductIds, int[] Quantities)
        {
            var cartItems = _shoppingCartService.GetItems();

            for (int i = 0; i < ProductIds.Length; i++)
            {
                int productId = ProductIds[i];
                int newQuantity = Quantities[i];

                // Find the corresponding cart item and update its quantity
                var cartItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity = newQuantity;
                }
            }
            _shoppingCartService.SaveItems(cartItems);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CouponApply(string couponcode)
        {
            var coupon = _dbContext.CouponCodes.FirstOrDefault(i => i.coupon.Equals(couponcode));

            if (coupon == null)
            {
                TempData["Error"] = "Invalid coupon code.";
                return RedirectToAction("Index");
            }

            if (!coupon.status || (coupon.expired_at.HasValue && coupon.expired_at.Value < DateTime.Now))
            {
                TempData["Error"] = "This coupon is expired or inactive.";
                return RedirectToAction("Index");
            }


            var orderAmount = _shoppingCartService.GetTotal();

            if (coupon.order_amount.HasValue && orderAmount < coupon.order_amount.Value)
            {
                TempData["Error"] = $"This coupon requires a minimum order amount of {coupon.order_amount.Value}.";
                return RedirectToAction("Index");
            }

            int discountAmount = coupon.percentage ? (orderAmount * coupon.discount / 100) : coupon.discount;

            Session["CouponCode"] = couponcode.ToUpper();
            Session["DiscountAmount"] = discountAmount;


            TempData["Success"] = "Coupon applied successfully.";
            return RedirectToAction("Index");
        }

        public ActionResult Checkout(OrderRequestModel orderRequest)
        {
            ViewBag.CartTotal = _shoppingCartService.GetTotal();
            ViewBag.DiscountAmount = _shoppingCartService.GetDiscountAmount();
            ViewBag.DiscountCoupon = _shoppingCartService.GetDiscountCoupon();
            return View(orderRequest);
        }

        [HttpPost]
        public ActionResult OrderSave(OrderRequestModel orderRequest)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Fill all the required fields";
                return RedirectToAction("Checkout", orderRequest);
            }

            var customer = _dbContext.Customers.FirstOrDefault(i => i.email.Equals(orderRequest.Email));
            if(customer == null)
            {
                var passwordHasher = new PasswordHasher();
                string hashedPassword = passwordHasher.HashPassword(orderRequest.Phone);
                var customerModel = new Customer
                {
                    name = orderRequest.Name,
                    email = orderRequest.Email,
                    phone = orderRequest.Phone,
                    address = orderRequest.Address,
                    password = hashedPassword
                };
                _dbContext.Customers.Add(customerModel);
                _dbContext.SaveChanges();
                customer = customerModel;
            }

            var order_shipping = new Shipping
            {
                customer_id = customer.id,
                name = orderRequest.Name,
                phone = orderRequest.Phone,
                email = orderRequest.Email,
                address = orderRequest.Address
            };
            _dbContext.Shippings.Add(order_shipping);
            _dbContext.SaveChanges();

            int order_amount = _shoppingCartService.GetTotal();
            int order_discount = _shoppingCartService.GetDiscountAmount();
            string order_coupon = _shoppingCartService.GetDiscountCoupon();
            List<CartItem> cartItem = _shoppingCartService.GetItems();

            var order = new Order
            {
                amount = order_amount,
                discount = order_discount,
                couponused = order_coupon,
                customer_id = customer.id,
                shipping_id = order_shipping.id,
                orderstatus = 1,
                note = orderRequest.Note,
                created_at = DateTime.Now
            };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            foreach(var item in cartItem)
            {
                var orderDetails = new OrderDetail
                {
                    order_id = order.id,
                    product_id = item.ProductId,
                    product_name = item.ProductName,
                    product_color = item.ProductColor,
                    product_size = item.ProductSize,
                    price = item.Price,
                    quantity = item.Quantity
                };
                _dbContext.OrderDetails.Add(orderDetails);
            }
            _dbContext.SaveChanges();

            _shoppingCartService.ClearCart();
            Session.Remove("CouponCode");
            Session.Remove("DiscountAmount");

            return RedirectToAction("OrderSuccess");
        }

    }
}