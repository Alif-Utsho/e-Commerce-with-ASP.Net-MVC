using ECMS.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.Entities
{
    public class CategoryProductModel
    {
        public CategoryProductModel()
        {
            this.Products = new HashSet<Product>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public Nullable<bool> front_view { get; set; }
        public Nullable<bool> status { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}