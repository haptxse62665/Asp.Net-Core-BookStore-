using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoMoi.DTOs
{
    public class InsertBookViewModel 
    {
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public int? BookBuyOnlineId { get; set; }
        public int? BookBuyOffileId { get; set; }
        public List<string> AuthorIds { get; set; }
    }
}
