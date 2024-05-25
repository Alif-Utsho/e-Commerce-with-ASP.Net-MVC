using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECMS.Models.Entities
{
    public class GeneralSettingModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public string favicon { get; set; }
        public bool status { get; set; }
    }
}