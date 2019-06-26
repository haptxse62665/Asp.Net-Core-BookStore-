using Api.Models;
using ChoMoi.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoAPI.Models
{
    public partial class Book : AuditableEntity<int>
    {
        public Book()
        {           
        }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }

        [ForeignKey("BookBuy")]
        public int? BookBuyOnlineId { get; set; }

        [ForeignKey("BookBuy")]
        public int? BookBuyOffileId { get; set; }

        public virtual BookBuy BookBuyOnline { get; set; }
        public virtual BookBuy BookBuyOffile { get; set; }

        public virtual BookCategory Category { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<BookAuthors> BookAuthors { get; set; }
    }
}
