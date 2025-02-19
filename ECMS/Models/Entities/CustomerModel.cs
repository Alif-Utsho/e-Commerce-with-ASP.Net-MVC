﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECMS.Models.Entities
{
    public class CustomerModel
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        [Required]
        public string address { get; set; }
        public string image { get; set; }
        public string password { get; set; }
        public Nullable<bool> status { get; set; }
    }
}