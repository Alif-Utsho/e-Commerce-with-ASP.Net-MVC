using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECMS.Models.RequestModel
{
    public class ProductRequestModel
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string slug { get; set; }
        [Required]
        public int category_id { get; set; }
        [Required]
        public Nullable<int> purchase_price { get; set; }
        public Nullable<int> old_price { get; set; }
        [Required]
        public int new_price { get; set; }
        public Nullable<int> discount { get; set; }
        [Required]
        public int stock { get; set; }
        public HttpFileCollectionWrapper thumbnail { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string additional_description { get; set; }
        public bool featured { get; set; }
        public bool status { get; set; }
        public List<HttpFileCollectionWrapper> images { get; set; }
        public List<int> sizes { get; set; }
        public List<int> colors { get; set; }
        public string brand { get; set; }

    }
}