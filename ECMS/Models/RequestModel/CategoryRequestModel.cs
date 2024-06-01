using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.RequestModel
{
    public class CategoryRequestModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public HttpFileCollectionWrapper image { get; set; }
        public string description { get; set; }
        public Nullable<bool> front_view { get; set; }
    }
}