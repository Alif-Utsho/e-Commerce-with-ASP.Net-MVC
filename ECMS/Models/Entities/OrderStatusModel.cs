using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.Entities
{
    public class OrderStatusModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public bool status { get; set; }
    }
}