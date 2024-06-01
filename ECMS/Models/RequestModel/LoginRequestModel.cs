using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECMS.Models.RequestModel
{
    public class LoginRequestModel
    {
        [Required]
        public string password { get; set; }
        [Required]
        public string email { get; set; }
    }
}