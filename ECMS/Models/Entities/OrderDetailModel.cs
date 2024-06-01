using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.Entities
{
    public class OrderDetailModel
    {
        public int id { get; set; }
        public int order_id { get; set; }
        public Nullable<int> product_id { get; set; }
        public string product_name { get; set; }
        public string product_color { get; set; }
        public string product_size { get; set; }
        public double price { get; set; }
        public double discount { get; set; }
        public int quantity { get; set; }
    }
}