using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.RequestModel
{
    public class CouponRequestModel
    {
        public int id { get; set; }
        public string coupon { get; set; }
        public int discount { get; set; }
        public bool percentage { get; set; }
        public bool allow_multiple { get; set; }
        public Nullable<System.DateTime> expired_at { get; set; }
        public bool status { get; set; }
        public Nullable<int> order_amount { get; set; }
    }
}