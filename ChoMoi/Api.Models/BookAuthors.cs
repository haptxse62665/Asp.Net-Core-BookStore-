using Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Models
{
    public partial class BookAuthors: AuditableEntity<int>
    {
        public int BookId { get; set; }
        public string UserId { get; set; }
        
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
    }
}
