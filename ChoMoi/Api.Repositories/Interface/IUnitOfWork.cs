using Api.Models;
using DemoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        BookStoreContext GetDataBase();
    }
}
