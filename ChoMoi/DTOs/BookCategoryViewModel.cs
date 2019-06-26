using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChoMoi.DTOs
{
    public class BookCategoryViewModel : AuditableEntity<int>
    {
        public string Name { get; set; }
        public bool? Status { get; set; }
    }
}
