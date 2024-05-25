﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.Entities
{
    public class ProductModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int category_id { get; set; }
        public string thumbnail { get; set; }
        public Nullable<int> purchase_price { get; set; }
        public Nullable<int> old_price { get; set; }
        public int new_price { get; set; }
        public int stock { get; set; }
        public Nullable<int> discount { get; set; }
        public string description { get; set; }
        public string additional_description { get; set; }
        public bool featured { get; set; }
        public bool status { get; set; }
    }
}