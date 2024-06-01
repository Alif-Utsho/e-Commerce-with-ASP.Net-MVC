using ECMS.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.Entities
{
    public class ProductDetailsModel
    {
        public ProductDetailsModel()
        {
            this.ProductColors = new HashSet<ProductColor>();
            this.ProductImages = new HashSet<ProductImage>();
            this.ProductSizes = new HashSet<ProductSize>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int category_id { get; set; }
        public Nullable<int> purchase_price { get; set; }
        public Nullable<int> old_price { get; set; }
        public int new_price { get; set; }
        public int stock { get; set; }
        public string description { get; set; }
        public string additional_description { get; set; }
        public bool featured { get; set; }
        public bool status { get; set; }
        public Nullable<int> discount { get; set; }
        public string thumbnail { get; set; }
        public Nullable<int> sold { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}