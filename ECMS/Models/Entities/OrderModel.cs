using ECMS.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.Entities
{
    public class OrderModel
    {
        public OrderModel()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        public int id { get; set; }
        public double amount { get; set; }
        public double discount { get; set; }
        public string couponused { get; set; }
        public int customer_id { get; set; }
        public int shipping_id { get; set; }
        public int orderstatus { get; set; }
        public string note { get; set; }
        public System.DateTime created_at { get; set; }
        public string tracking { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual OrderStatus OrderStatus1 { get; set; }
        public virtual Shipping Shipping { get; set; }
    }
}