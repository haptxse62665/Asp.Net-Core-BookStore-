using Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Models
{
    public partial class Publisher : AuditableEntity<int>
    {
        public Publisher()
        {
            Book = new HashSet<Book>();
        }
        public string Name { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Book> Book { get; set; }
    }
}
