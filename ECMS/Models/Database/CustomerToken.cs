//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECMS.Models.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustomerToken
    {
        public int id { get; set; }
        public int customer_id { get; set; }
        public string token { get; set; }
        public bool expired { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
