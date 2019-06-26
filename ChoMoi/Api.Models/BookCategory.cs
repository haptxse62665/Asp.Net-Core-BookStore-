using Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoAPI.Models
{
    public partial class BookCategory: AuditableEntity<int>
    {
        public BookCategory()
        {
            Book = new HashSet<Book>();
        }
        public string Name { get; set; }

        public virtual ICollection<Book> Book { get; set; }
    }
}
