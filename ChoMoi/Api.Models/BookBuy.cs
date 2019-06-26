using Api.Models;
using DemoAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChoMoi.Api.Models
{
    public partial class BookBuy : AuditableEntity<int>
    {
        public BookBuy()
        {
        }
        public string BuyFrom { get; set; }

        [InverseProperty("BookBuyOnline")]
        public virtual ICollection<Book> BookBuyOnline { get; set; }


        [InverseProperty("BookBuyOffile")]
        public virtual ICollection<Book> BookBuyOffile { get; set; }
    }
}
