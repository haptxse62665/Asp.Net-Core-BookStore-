using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoMoi.DTOs
{
    public class PaginationViewModel<T>
    {
        public List<T> Data { get; set; }
        public int Amount { get; set; }
        public int TotalPage { get; set; }
        public int TotalCount { get; set; }
    }
}
