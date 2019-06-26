using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
