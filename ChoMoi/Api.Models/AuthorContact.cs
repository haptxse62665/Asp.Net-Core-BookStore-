using Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Models
{
    public partial class AuthorContact : AuditableEntity<int>
    {
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public bool? Status { get; set; }


        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
