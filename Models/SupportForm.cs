//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NTSDCES.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SupportForm
    {
        public int FormID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Discribe { get; set; }
        public string Images { get; set; }
        public int AccountID { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
